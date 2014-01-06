namespace EnergyTrading.Logging.Log4Net
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using EnergyTrading.Crypto;

    using log4net.Appender;
    using log4net.Core;

    /// <summary>
    /// ADO appender that operates asynchronously.
    /// </summary>
    /// <remarks>Adapted from http://technico.qnownow.com/asynchronous-ado-net-appender-in-log4net-c/ </remarks>
    public class AsynchronousAdoNetAppender : AdoNetAppender
    {
        private readonly ManualResetEvent quitHandle;
        private readonly AutoResetEvent taskHandle;
        private readonly ConcurrentQueue<LoggingEvent> pendingTasks;
        private bool onClosing;

        public AsynchronousAdoNetAppender()
        {
            this.pendingTasks = new ConcurrentQueue<LoggingEvent>();
            this.taskHandle = new AutoResetEvent(false);
            this.quitHandle = new ManualResetEvent(false);

            // Start the async process of handling pending tasks  
            this.Start();
        }
               
        protected override void Append(LoggingEvent[] loggingEvents)
        {
            foreach (var loggingEvent in loggingEvents)
            {
                this.Append(loggingEvent);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!this.FilterEvent(loggingEvent))
            {
                return;
            }

            this.pendingTasks.Enqueue(loggingEvent);

            // Tell the appending thread we have some
            this.taskHandle.Set();
        }

        /// <summary>
        /// OnClose method is called, when the shut down of the repository is invoked.
        /// </summary>
        protected override void OnClose()
        {
            // set the OnClosing flag to true, so that AppendLoggingEvents would   
            // know it is time to wrap up whatever it is doing  
            this.onClosing = true;

            // wait till we receive signal from manualResetEvent 
            // which is signalled from AppendLoggingEvents   
            this.quitHandle.WaitOne(TimeSpan.FromSeconds(10));
 
            base.OnClose();
        }

        private void Start()
        {
            // hopefully user doesnt open and close the GUI or CONSOLE OR WEBPAGE   
            // right away anyway lets add that condition too  
            if (this.onClosing)
            {
                return;
            }

            this.quitHandle.Reset();
            var thread = new Thread(this.LogMessages);
            thread.Start();
        }

        private void LogMessages()
        {
            // We keep on processing tasks until shutdown on repository is called  
            while (!this.onClosing)
            {
                // Sleep for a while or until we get a task.
                this.taskHandle.WaitOne(TimeSpan.FromSeconds(5));

                this.FlushQueue();
            }

            // Flush one last time in case we have some others
            this.FlushQueue();

            // We are done with our logging, sent the signal to the parent thread  
            // so that it can commence shut down  
            this.quitHandle.Set();
        }

        private void FlushQueue()
        {
            // Process the queue until empty 
            LoggingEvent loggingEvent;
            while (this.pendingTasks.TryDequeue(out loggingEvent))
            {
                // Write it out
                base.Append(loggingEvent);
            }
        }

        /// <summary>
        /// Decrypt password in the connection string
        /// </summary>
        public new string ConnectionString
        {
            get { return base.ConnectionString; }
            set { base.ConnectionString = value.DecryptConnectionString(); }
        }
    }
}