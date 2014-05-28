namespace EnergyTrading.FileProcessing.FileProcessors
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EnergyTrading.FileProcessing.FileHandling;
    using EnergyTrading.Logging;
    using EnergyTrading.ProducerConsumer;

    public class PollingBasedFileProcessor : Disposable, IFileProcessor
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger<PollingBasedFileProcessor>();

        private readonly AutoResetEvent processStalledFilesEvent = new AutoResetEvent(true);
        private readonly FileProcessorEndpoint fileProcessorEndpoint;
        private readonly IHandleFiles fileHandler;

        private readonly IFileFilter additionalFilter;
        private AutoResetEvent processFilesSignal;
        private Task[] processingTasks;
        private FileSystemWatcher watcher;
        private bool stopped;

        public PollingBasedFileProcessor(FileProcessorEndpoint fileProcessorEndpoint, IHandleFiles fileHandler, IFileFilter additionalFilter)
        {
            if (fileProcessorEndpoint == null) { throw new ArgumentNullException("fileProcessorEndpoint"); }
            if (fileHandler == null) { throw new ArgumentNullException("fileHandler"); }
            if (additionalFilter == null) { throw new ArgumentNullException("additionalFilter"); }

            this.fileProcessorEndpoint = fileProcessorEndpoint;
            this.fileHandler = fileHandler;
            this.additionalFilter = additionalFilter;
        }

        public string Name { get; private set; }

        public void Start()
        {
            this.ThrowIfDisposed();

            Logger.Debug("Starting");

            this.stopped = false;

            this.processFilesSignal = new AutoResetEvent(true);

            this.processingTasks = new[]
                                {
                                    Task.Factory.StartNew(this.ProcessFiles)
                                };

            this.StartWatchingForNewFiles();
        }

        public void Stop()
        {
            Logger.Debug("Stopping file watcher");
            this.DisposeManagedResources();
        }

        public void Restart()
        {
            this.ThrowIfDisposed();

            Logger.Debug("Restarting file watcher");

            this.DisposeWatcher();
            this.StartWatchingForNewFiles();
            this.processFilesSignal.Set();
        }

        private void ProcessFiles()
        {
            while (true)
            {
                this.processFilesSignal.WaitOne(this.GetSleepInterval());
                if (this.stopped)
                {
                    break;
                }

                this.ProcessCurrentFiles();

                if (this.stopped)
                {
                    break;
                }
            }

            Logger.Info("File processing stopped");
        }

        private void ProcessCurrentFiles()
        {
            Logger.Debug("Process signal fired, looking for files to process");
            var random = new Random();

            var dropDirectory = new DirectoryInfo(this.fileProcessorEndpoint.DropPath);
            foreach (var sourceFile in dropDirectory.EnumerateFiles().OrderBy(x => x.LastAccessTimeUtc))
            {
                Logger.DebugFormat("Looking at file: {0}", sourceFile.Name);

                if (this.stopped)
                {
                    break;
                }

                if (!this.additionalFilter.IncludeFile(sourceFile.FullName))
                {
                    continue;
                }

                var originalFilename = sourceFile.Name;
                var inprogressFilePath = Path.Combine(this.fileProcessorEndpoint.InProgressPath, string.Format("{0}.{1}", originalFilename, DateTime.UtcNow.Ticks));
                // save this here because it changes when we call FileInfo.MoveTo in TryTakePossessionOfFile and we need the original path to be passed correctly when we notify
                var originalFullPathToFile = sourceFile.FullName;

                try
                {
                    if (this.TryTakePossessionOfFile(sourceFile, inprogressFilePath))
                    {
                        this.fileHandler.Notify(new ProcessingFile(inprogressFilePath, originalFilename, originalFullPathToFile));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("Attempt to take possession of file {0} failed.", originalFilename), ex);
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(10, 500)));
            }
        }

        private bool TryTakePossessionOfFile(FileInfo file, string inprogressFilePath)
        {
            const int MaxTries = 3;
            var tries = 0;
            do
            {
                tries++;
                try
                {
                    file.MoveTo(inprogressFilePath);
                    Logger.DebugFormat("Possession of file: {0}", inprogressFilePath);
                    return true;
                }
                catch (FileNotFoundException)
                {
                    Logger.DebugFormat("Cannot take possession of file - not found {0}", inprogressFilePath);
                    return false;
                }
                catch (IOException)
                {
                    Logger.DebugFormat("Cannot take possession of file - attempt {0}/{1} {2}", tries, MaxTries, inprogressFilePath);
                    Thread.Sleep(1000);
                }
            }
            while (tries != MaxTries);

            return false;
        }

        private TimeSpan GetSleepInterval()
        {
            int numberOfFiles = Directory.GetFiles(this.fileProcessorEndpoint.DropPath).Length;
            return numberOfFiles == 0 ? TimeSpan.FromMilliseconds(-1) : TimeSpan.FromSeconds(10);
        }

        private void StartWatchingForNewFiles()
        {
            Logger.Debug("Starting file listener");

            this.watcher = new FileSystemWatcher(this.fileProcessorEndpoint.DropPath)
            {
                IncludeSubdirectories = this.fileProcessorEndpoint.MonitorSubdirectories
            };
            this.watcher.Created += (sender, args) =>
            {
                Logger.DebugFormat("New file detected: {0}", args.Name);
                this.processFilesSignal.Set();
            };
            this.watcher.EnableRaisingEvents = true;
        }

        protected override void DisposeManagedResources()
        {
            this.DisposeWatcher();
            this.StopProcessingFiles();

            if (this.processFilesSignal != null)
            {
                this.processFilesSignal.Dispose();
            }
        }

        private void DisposeWatcher()
        {
            if (this.watcher != null)
            {
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Dispose();
            }
        }

        private void StopProcessingFiles()
        {
            this.stopped = true;
            if (this.processingTasks != null)
            {
                this.processFilesSignal.Set();
                this.processStalledFilesEvent.Set();
                Task.WaitAll(this.processingTasks);
            }
        }
    }
}