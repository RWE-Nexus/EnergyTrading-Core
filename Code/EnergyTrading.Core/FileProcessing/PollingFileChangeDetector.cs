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
            watcher = new FileSystemWatcher(Path, Filter);
            watcher.InternalBufferSize = 4 * 4096; //1024768;  // Try to avoid missing events
            watcher.Created += FileNotification;
            watcher.Renamed += FileNotification;
            watcher.IncludeSubdirectories = MonitorSubdirectories;
            watcher.EnableRaisingEvents = true;

            Logger.DebugFormat(
                "Now watching for any new files dropped at {0}{1} with filter {2}",
                Path,
                MonitorSubdirectories ? " and subdirectories" : string.Empty,
                Filter);
        }

        /// <summary>
        /// Stop the watcher
        /// </summary>
        public void Stop()
        {
            Logger.Debug("Stopping File Watcher");
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            watcher = null;
        }

        private void FileNotification(object sender, FileSystemEventArgs e)
        {
            var handler = Changed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private TimeSpan GetSleepInterval()
        {
            var numberOfFiles = Directory.GetFiles(Path).Length;
            return numberOfFiles == 0 ? TimeSpan.FromMilliseconds(-1) : TimeSpan.FromSeconds(10);
        }
    }
}
