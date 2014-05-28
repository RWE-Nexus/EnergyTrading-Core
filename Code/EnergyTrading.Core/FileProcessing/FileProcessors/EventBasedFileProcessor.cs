namespace EnergyTrading.FileProcessing.FileProcessors
{
    using System;
    using System.IO;
    using System.Reflection;

    using EnergyTrading.FileProcessing.Configuration;
    using EnergyTrading.Logging;

    /// <summary>
    /// Provides a processor that listens for files and then passes them onto a handler.
    /// <para>
    /// Caters for issues such as IO and network errors and if operating against NTFS performs atomic operations.
    /// </para>
    /// </summary>
    public class EventBasedFileProcessor : FileProcessorBase
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private FileSystemWatcher watcher;

        public EventBasedFileProcessor(FileProcessorEndpoint endpoint, IFileHandler handler, IFilePostProcessor postProcessor, IFileFilter additionalFilter)
            : base(endpoint, handler, postProcessor, additionalFilter)
        {
        }

        protected FileSystemWatcher Watcher
        {
            get { return this.watcher; }
        }

        protected override void StartCheckingForNewFiles()
        {
            Logger.Debug("Starting File Watcher");
            this.StartFileWatcher();
        }

        protected override void StopCheckingForNewFiles()
        {
            Logger.Debug("Stopping File Watcher");
            this.StopFileWatcher();
        }



        private void StartFileWatcher()
        {
            this.watcher = new FileSystemWatcher(this.Endpoint.GetDropPath(), this.Endpoint.GetDropFilter());
            this.watcher.InternalBufferSize = 4 * 4096; //1024768;  // Try to avoid missing events
            this.watcher.Created += this.NewFileNotification;
            this.watcher.Renamed += this.NewFileNotification;
            this.watcher.IncludeSubdirectories = this.Endpoint.MonitorSubdirectories;
            this.watcher.EnableRaisingEvents = true;

            Logger.DebugFormat(
                "Now watching for any new files dropped at {0}{1} with filter {2}",
                this.Endpoint.GetDropPath(),
                this.Endpoint.MonitorSubdirectories ? " and subdirectories" : string.Empty,
                this.Endpoint.GetDropFilter());
        }

        private void StopFileWatcher()
        {
            Logger.Debug("Stopping File Watcher");
            if (this.watcher != null)
            {
                this.watcher.EnableRaisingEvents = false;
            }
            this.watcher = null;
        }

        private void NewFileNotification(object sender, FileSystemEventArgs e)
        {
            try
            {
                var fileName = Path.GetFileName(e.FullPath);
                Logger.Debug("Notified: " + fileName);

                // we get a notification when the file is moved so we need to ignore notifications for inprogress extensions
                if (e.FullPath.EndsWith(InProgress))
                {
                    Logger.Debug("Ignoring: " + fileName);
                    return;
                }

                if (!this.AdditionalFilter.IncludeFile(e.FullPath))
                {
                    Logger.Debug("Skipping file excluded by additional Filter " + fileName);
                    return;
                }
                this.AddFileToQueue(e.FullPath);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception in ProcessFile : " + ex.Message);
            }
        }
    }
}