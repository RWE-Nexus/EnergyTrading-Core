namespace EnergyTrading.Logging.Log4Net
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using log4net.Appender;
    using log4net.Core;

    using EnergyTrading.Crypto;

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
            pendingTasks = new ConcurrentQueue<LoggingEvent>();
            taskHandle = new AutoResetEvent(false);
            quitHandle = new ManualResetEvent(false);

            // Start the async process of handling pending tasks  
            Start();
        }
               
        protected override void Append(LoggingEvent[] loggingEvents)
        {
            foreach (var loggingEvent in loggingEvents)
            {
                Append(loggingEvent);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!FilterEvent(loggingEvent))
            {
                return;
            }

            pendingTasks.Enqueue(loggingEvent);

            // Tell the appending thread we have some
            taskHandle.Set();
        }

        /// <summary>
        /// OnClose method is called, when the shut down of the repository is invoked.
        /// </summary>
        protected override void OnClose()
        {
            // set the OnClosing flag to true, so that AppendLoggingEvents would   
            // know it is time to wrap up whatever it is doing  
            onClosing = true;

            // wait till we receive signal from manualResetEvent 
            // which is signalled from AppendLoggingEvents   
            quitHandle.WaitOne(TimeSpan.FromSeconds(10));
 
            base.OnClose();
        }

        private void Start()
        {
            // hopefully user doesnt open and close the GUI or CONSOLE OR WEBPAGE   
            // right away anyway lets add that condition too  
            if (onClosing)
            {
                return;
            }

            quitHandle.Reset();
            var thread = new Thread(LogMessages);
            thread.Start();
        }

        private void LogMessages()
        {
            // We keep on processing tasks until shutdown on repository is called  
            while (!onClosing)
            {
                // Sleep for a while or until we get a task.
                taskHandle.WaitOne(TimeSpan.FromSeconds(5));

                FlushQueue();
            }

            // Flush one last time in case we have some others
            FlushQueue();

            // We are done with our logging, sent the signal to the parent thread  
            // so that it can commence shut down  
            quitHandle.Set();
        }

        private void FlushQueue()
        {
            // Process the queue until empty 
            LoggingEvent loggingEvent;
            while (pendingTasks.TryDequeue(out loggingEvent))
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