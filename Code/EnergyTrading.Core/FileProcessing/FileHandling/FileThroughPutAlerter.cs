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

            this.timer = new Timer(interval.TotalMilliseconds) { AutoReset = true };
            this.timer.Elapsed += (sender, args) =>
            {
                Logger.DebugFormat("No activity in the FileSystemEventListener for {0:0.0} seconds. Restarting as a precaution", interval.TotalSeconds);
                this.OnAlert();
            };
            this.timer.Start();
        }

        public event EventHandler Alert;

        public void Notify(ProcessingFile processingFile)
        {
            this.ThrowIfDisposed();

            this.timer.Stop();
            this.fileHandler.Notify(processingFile);

            if (!this.stopAlerting)
            {
                this.timer.Start();
            }
        }

        public void StopAlerts()
        {
            this.stopAlerting = true;
        }

        protected override void DisposeManagedResources()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }

        private void OnAlert()
        {
            var handler = this.Alert;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}