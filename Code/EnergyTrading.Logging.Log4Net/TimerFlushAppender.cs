namespace EnergyTrading.Logging.Log4Net
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Timers;

    using log4net.Appender;
    using log4net.Core;
    using log4net.Util;

    using Timer = System.Timers.Timer;

    public class TimerFlushAppender : AppenderSkeleton, IAppenderAttachable
    {
        private readonly object lockObject;
        private readonly Timer timer;
        private int syncPoint;
        private int interval;

        public TimerFlushAppender()
        {
            lockObject = new object();
            this.timer = new Timer { AutoReset = true };
            this.timer.Elapsed += this.TimerElapsed;
        }

        public int Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
                if (this.interval > 0)
                {
                    this.timer.Stop();
                }
                this.interval = value;
                if (this.interval > 0)
                {
                    this.timer.Interval = this.interval * 1000;
                    this.timer.Start();
                }
            }
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        protected void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            // prevent timer event from running if there is one already going
            var sync = Interlocked.CompareExchange(ref this.syncPoint, 1, 0);
            if (sync != 0)
            {
                return;
            }

            try
            {
                lock (this.lockObject)
                {
                    foreach (var bufferedappender in this.Appenders.OfType<BufferingAppenderSkeleton>())
                    {
                        bufferedappender.Flush(true);
                    }
                }
            }
            catch
            {
            }
            this.syncPoint = 0;
        }

        private bool IsInTimeWindow()
        {
            if ((StartTime == default(DateTime)) || (EndTime == default(DateTime)))
            {
                return true;
            }

            var now = DateTime.UtcNow;
            if (EndTime.TimeOfDay < StartTime.TimeOfDay)
            {
                return (now.TimeOfDay >= StartTime.TimeOfDay) || (now.TimeOfDay < EndTime.TimeOfDay);
            }

            return (now.TimeOfDay >= StartTime.TimeOfDay) && (now.TimeOfDay < EndTime.TimeOfDay);
        }

        override protected void OnClose()
        {
            lock (this.lockObject)
            {
                if (this.appenderAttachedImpl != null)
                {
                    this.appenderAttachedImpl.RemoveAllAppenders();
                }
            }
        }

        override protected void Append(LoggingEvent loggingEvent)
        {
            if (this.appenderAttachedImpl != null && IsInTimeWindow())
            {
                this.appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
            }
        }

        /// <summary>
        /// Forward the logging events to the attached appenders 
        /// </summary>
        /// <param name="loggingEvents">The array of events to log.</param>
        /// <remarks>
        /// <para>
        /// Delivers the logging events to all the attached appenders.
        /// </para>
        /// </remarks>
        override protected void Append(LoggingEvent[] loggingEvents)
        {
            // Pass the logging event on the the attached appenders
            if (this.appenderAttachedImpl != null && IsInTimeWindow())
            {
                this.appenderAttachedImpl.AppendLoopOnAppenders(loggingEvents);
            }
        }

        /// <summary>
        /// Adds an <see cref="IAppender" /> to the list of appenders of this
        /// instance.
        /// </summary>
        /// <param name="newAppender">The <see cref="IAppender" /> to add to this appender.</param>
        /// <remarks>
        /// <para>
        /// If the specified <see cref="IAppender" /> is already in the list of
        /// appenders, then it won't be added again.
        /// </para>
        /// </remarks>
        virtual public void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }
            lock (this.lockObject)
            {
                if (this.appenderAttachedImpl == null)
                {
                    this.appenderAttachedImpl = new AppenderAttachedImpl();
                }
                this.appenderAttachedImpl.AddAppender(newAppender);
            }
        }

        /// <summary>
        /// Gets the appenders contained in this appender as an 
        /// <see cref="System.Collections.ICollection"/>.
        /// </summary>
        /// <remarks>
        /// If no appenders can be found, then an <see cref="EmptyCollection"/> 
        /// is returned.
        /// </remarks>
        /// <returns>
        /// A collection of the appenders in this appender.
        /// </returns>
        virtual public AppenderCollection Appenders
        {
            get
            {
                lock (this.lockObject)
                {
                    if (this.appenderAttachedImpl == null)
                    {
                        return AppenderCollection.EmptyCollection;
                    }

                    return this.appenderAttachedImpl.Appenders;
                }
            }
        }

        /// <summary>
        /// Looks for the appender with the specified name.
        /// </summary>
        /// <param name="name">The name of the appender to lookup.</param>
        /// <returns>
        /// The appender with the specified name, or <c>null</c>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Get the named appender attached to this appender.
        /// </para>
        /// </remarks>
        virtual public IAppender GetAppender(string name)
        {
            lock (this)
            {
                if (this.appenderAttachedImpl == null || name == null)
                {
                    return null;
                }

                return this.appenderAttachedImpl.GetAppender(name);
            }
        }

        /// <summary>
        /// Removes all previously added appenders from this appender.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is useful when re-reading configuration information.
        /// </para>
        /// </remarks>
        virtual public void RemoveAllAppenders()
        {
            lock (this.lockObject)
            {
                if (this.appenderAttachedImpl != null)
                {
                    this.appenderAttachedImpl.RemoveAllAppenders();
                    this.appenderAttachedImpl = null;
                }
            }
        }

        /// <summary>
        /// Removes the specified appender from the list of appenders.
        /// </summary>
        /// <param name="appender">The appender to remove.</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </remarks>
        virtual public IAppender RemoveAppender(IAppender appender)
        {
            lock (this.lockObject)
            {
                if (appender != null && this.appenderAttachedImpl != null)
                {
                    return this.appenderAttachedImpl.RemoveAppender(appender);
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the appender with the specified name from the list of appenders.
        /// </summary>
        /// <param name="name">The name of the appender to remove.</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </remarks>
        virtual public IAppender RemoveAppender(string name)
        {
            lock (this.lockObject)
            {
                if (name != null && this.appenderAttachedImpl != null)
                {
                    return this.appenderAttachedImpl.RemoveAppender(name);
                }
            }
            return null;
        }

        /// <summary>
        /// Implementation of the <see cref="IAppenderAttachable"/> interface
        /// </summary>
        private AppenderAttachedImpl appenderAttachedImpl;
    }
}
