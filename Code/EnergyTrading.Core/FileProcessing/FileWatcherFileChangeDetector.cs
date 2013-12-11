namespace EnergyTrading.FileProcessing
{
    using System.IO;
    using System.Reflection;

    using EnergyTrading.Logging;

    /// <summary>
    /// File change detector using a <see cref="FileSystemWatcher" />
    /// </summary>
    public class FileWatcherFileChangeDetector : IFileChangeDetector
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private FileSystemWatcher watcher;

        /// <copydocfrom cref="IFileChangeDetector.Changed" />
        public event FileSystemEventHandler Changed;

        /// <copydocfrom cref="IFileChangeDetector.Path" />
        public string Path { get; set; }

        /// <copydocfrom cref="IFileChangeDetector.Filter" />
        public string Filter { get; set; }

        /// <copydocfrom cref="IFileChangeDetector.MonitorSubdirectories" />
        public bool MonitorSubdirectories { get; set; }

        /// <summary>
        /// Start the watcher
        /// </summary>
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
    }
}
