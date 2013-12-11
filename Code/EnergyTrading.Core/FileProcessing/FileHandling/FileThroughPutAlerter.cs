namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;
    using System.Reflection;
    using System.Timers;

    using EnergyTrading.Logging;
    using EnergyTrading.ProducerConsumer;

    public class FileThroughPutAlerter : Disposable, IHandleFiles
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IHandleFiles fileHandler;
        private readonly Timer timer;
        private bool stopAlerting;

        public FileThroughPutAlerter(IHandleFiles fileHandler, TimeSpan interval)
        {
            this.fileHandler = fileHandler;

            timer = new Timer(interval.TotalMilliseconds) { AutoReset = true };
            timer.Elapsed += (sender, args) =>
            {
                Logger.DebugFormat("No activity in the FileSystemEventListener for {0:0.0} seconds. Restarting as a precaution", interval.TotalSeconds);
                OnAlert();
            };
            timer.Start();
        }

        public event EventHandler Alert;

        public void Notify(ProcessingFile processingFile)
        {
            ThrowIfDisposed();

            timer.Stop();
            fileHandler.Notify(processingFile);

            if (!stopAlerting)
            {
                timer.Start();
            }
        }

        public void StopAlerts()
        {
            stopAlerting = true;
        }

        protected override void DisposeManagedResources()
        {
            timer.Stop();
            timer.Dispose();
        }

        private void OnAlert()
        {
            var handler = Alert;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}