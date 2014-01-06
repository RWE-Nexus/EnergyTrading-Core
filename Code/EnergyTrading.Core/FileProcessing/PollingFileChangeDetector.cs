namespace EnergyTrading.FileProcessing
{
    using System;
    using System.IO;
    using System.Reflection;

    using EnergyTrading.Logging;

    /// <summary>
    /// A file change detector using polling to find files.
    /// </summary>
    public class PollingFileChangeDetector : IFileChangeDetector
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private FileSystemWatcher watcher;

        public event FileSystemEventHandler Changed;

        public string Path { get; set; }

        public string Filter { get; set; }

        public bool MonitorSubdirectories { get; set; }

        public void Start()
        {
            this.watcher = new FileSystemWatcher(this.Path, this.Filter);
            this.watcher.InternalBufferSize = 4 * 4096; //1024768;  // Try to avoid missing events
            this.watcher.Created += this.FileNotification;
            this.watcher.Renamed += this.FileNotification;
            this.watcher.IncludeSubdirectories = this.MonitorSubdirectories;
            this.watcher.EnableRaisingEvents = true;

            Logger.DebugFormat(
                "Now watching for any new files dropped at {0}{1} with filter {2}",
                this.Path,
                this.MonitorSubdirectories ? " and subdirectories" : string.Empty,
                this.Filter);
        }

        /// <summary>
        /// Stop the watcher
        /// </summary>
        public void Stop()
        {
            Logger.Debug("Stopping File Watcher");
            if (this.watcher != null)
            {
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Dispose();
            }
            this.watcher = null;
        }

        private void FileNotification(object sender, FileSystemEventArgs e)
        {
            var handler = this.Changed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private TimeSpan GetSleepInterval()
        {
            var numberOfFiles = Directory.GetFiles(this.Path).Length;
            return numberOfFiles == 0 ? TimeSpan.FromMilliseconds(-1) : TimeSpan.FromSeconds(10);
        }
    }
}
