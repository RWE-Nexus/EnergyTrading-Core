namespace EnergyTrading.FileProcessing.FileProcessors
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    using EnergyTrading.FileProcessing.Configuration;
    using EnergyTrading.Logging;
    using EnergyTrading.Threading;

    public abstract class FileProcessorBase : IFileProcessor
    {
        public const string InProgress = "inprogress";

        private FileScavenger scavenger;
        private Guid instanceId;
        private readonly ConcurrentQueue<string> pendingFiles;
        private readonly AutoResetEvent eventHandle;
        private readonly ManualResetEvent quitHandle;
        private Thread processingThread;
        private bool quitting;

        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected FileProcessorEndpoint Endpoint { get; set; }

        protected IFileHandler Handler { get; private set; }

        protected IFilePostProcessor PostProcessor { get; private set; }

        protected IFileFilter AdditionalFilter { get; private set; }

        protected FileProcessorBase(FileProcessorEndpoint endpoint, IFileHandler handler, IFilePostProcessor postProcessor, IFileFilter additionalFilter)
        {
            if (endpoint == null)
            {
                throw new ArgumentException("endpoint");
            }

            if (handler == null)
            {
                throw new ArgumentException("handler");
            }

            if (postProcessor == null)
            {
                throw new ArgumentNullException("postProcessor");
            }

            if (additionalFilter == null)
            {
                throw new ArgumentNullException("additionalFilter");
            }

            this.instanceId = Guid.Empty;
            this.Endpoint = endpoint;
            this.Handler = handler;
            this.PostProcessor = postProcessor;
            this.AdditionalFilter = additionalFilter;
            this.scavenger = new FileScavenger(endpoint, additionalFilter, this.InstanceId);
            this.scavenger.RestartFileChecking += (o, e) => this.RestartNewFileChecking();


            this.pendingFiles = new ConcurrentQueue<string>();
            this.eventHandle = new AutoResetEvent(false);
            this.quitHandle = new ManualResetEvent(false);
            this.quitting = false;

            Logger.DebugFormat("Scavenger interval set at {0:0.0} seconds", endpoint.ScavengeInterval.TotalSeconds);
        }

        protected virtual string InstanceId
        {
            get
            {
                if (this.instanceId == Guid.Empty)
                {
                    this.instanceId = Guid.NewGuid();
                }
                return this.instanceId.ToString().Replace("-", String.Empty);
            }
        }

        private void RestartNewFileChecking()
        {
            this.StopCheckingForNewFiles();
            this.StartCheckingForNewFiles();
        }

        public void Start()
        {
            this.Stop();
            Logger.Debug("Starting");

            // we are not quitting
            this.quitting = false;
            this.quitHandle.Reset();

            // tell the processor to start adding files to the queue
            this.StartCheckingForNewFiles();

            // start scavenger
            this.scavenger.Start();

            // start processing files
            this.processingThread = new Thread(this.ProcessFiles);
            this.processingThread.Start();

            Logger.Debug("Started");
        }

        public void Stop()
        {
            Logger.Debug("Stopping");

            // tell the processor to stop adding files to the queue
            this.StopCheckingForNewFiles();

            // stop Scavenger
            this.scavenger.Stop();

            // mark as quitting and stop processing files
            this.quitting = true;
            if (this.processingThread != null)
            {
                // wait till we receive signal from ProcessingFiles
                // TODO: Might want this configurable - Endpoint
                this.quitHandle.WaitOne(TimeSpan.FromSeconds(10));

                this.processingThread = null;
            }
            Logger.Debug("Stopped");
        }

        public string Name 
        {
            get { return this.Endpoint.Name; }
        }

        protected void AddFileToQueue(string completeFilePath)
        {
            Logger.Debug("Enqueing: " + completeFilePath);
            this.pendingFiles.Enqueue(completeFilePath);
            this.eventHandle.Set();
        }

        private void ProcessFiles()
        {
            while (!this.quitting)
            {
                // Sleep for a while or until we get a task.
                this.eventHandle.WaitOne(TimeSpan.FromSeconds(5));

                // Process the queue until empty 
                string fileName;
                while (this.pendingFiles.TryDequeue(out fileName))
                {
                    // Write it out
                    var originalFilePath = fileName;
                    string fileProcessPath;

                    // Don't both re-queueing if we can't take possession
                    // may be another copy of us has grabbed it
                    // Otherwise scavanger thread will eventually re-queue it.
                    if (this.TakePossessionOfFileForProcessing(fileName, out fileProcessPath) && File.Exists(fileProcessPath))
                    {
                        // TODO: Look at doing producer/consumer to hand off to multiple handler instances.
                        Logger.Debug("Processing " + fileProcessPath);
                        var info = new FileInfo(fileProcessPath);
                        if (this.HandleFile(info, originalFilePath))
                        {
                            this.ProcessSuccessfulFile(info, originalFilePath);
                        }
                        else
                        {
                            this.ProcessFailedFile(info, originalFilePath);
                        }
                    }

                    if (this.quitting)
                    {
                        // Exit soon as we know.
                        break;
                    }
                }
            }

            this.quitHandle.Set();
        }

        private bool TakePossessionOfFileForProcessing(string sourcePath, out string processPath)
        {
            Logger.Debug("Acquiring: " + Path.GetFileName(sourcePath));

            processPath = String.Empty;
            var inprogressFile = Path.ChangeExtension(sourcePath, "." + this.InstanceId + InProgress);
            var filelocked = true;
            var canProcessFile = false;

            while (filelocked)
            {
                try
                {
                    File.Move(sourcePath, inprogressFile);
                    processPath = inprogressFile;
                    filelocked = false;
                    canProcessFile = true;
                }
                catch (FileNotFoundException)
                {
                    filelocked = false;
                }
                catch (IOException ioe)
                {
                    if (IsFileLockedError(ioe))
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        Logger.Debug("No longer processing " + sourcePath + " unexpected IOException: " + ioe.Message);
                        filelocked = false;
                    }
                }
            }

            return canProcessFile;
        }

        private static bool IsFileLockedError(Exception exception)
        {
            var code = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return code == 32 || code == 33;
        }

        private bool HandleFile(FileInfo fileInfo, string originalFilePath)
        {
            Logger.Debug("Handling: " + fileInfo);
            try
            {
                var handleFullPath = this.Handler as IFileHandlerFullPath;
                return handleFullPath != null ? handleFullPath.HandleWithFullPath(fileInfo, originalFilePath) : this.Handler.Handle(fileInfo, Path.GetFileName(originalFilePath));
            }
            catch (Exception e)
            {
                Logger.Error("Unhandled error in file handler", e);
                return false;
            }
        }

        /// <summary>
        /// Logic for processing a successful file
        /// </summary>
        /// <param name="fileInfo">File info of the successful file.</param>
        /// <param name="originalFilePath"></param>
        protected virtual void ProcessSuccessfulFile(FileInfo fileInfo, string originalFilePath)
        {
            this.ProcessResultFile(fileInfo, this.GenerateSuccessFileName(originalFilePath), "File Process Success", originalFilePath);
        }

        /// <summary>
        /// Logic for processing a failed file
        /// </summary>
        protected virtual void ProcessFailedFile(FileInfo fileInfo, string originalFilePath)
        {
            this.ProcessResultFile(fileInfo, this.GenerateErrorFileName(originalFilePath), "File Process Failure", originalFilePath);
        }

        protected void CreateDirectories(string filepath)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(filepath));
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        protected virtual string GenerateFileName(string outputPath, string initialsuffix, string originalFilePath)
        {
            var dir = outputPath;
            var dirpath = this.GetPathWithoutWatchedFolder(originalFilePath);
            if (outputPath.Contains("%filepath%"))
            {
                dir = outputPath.Replace("%filepath%", Path.GetDirectoryName(originalFilePath));
                dirpath = Path.GetFileName(originalFilePath);
            }
            var errorFileName = dirpath + ".suc_" + DateTime.Now.Ticks;
            return Path.Combine(dir, errorFileName);
        }

        private string GetPathWithoutWatchedFolder(string path)
        {
            return path.Substring(Endpoint.GetDropPath().EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) ? Endpoint.GetDropPath().Length : Endpoint.GetDropPath().Length + 1);
        }

        protected virtual string GenerateSuccessFileName(string originalFilePath)
        {
            return this.GenerateFileName(Endpoint.GetSuccessPath(), "suc_", originalFilePath);
        }

        protected virtual string GenerateErrorFileName(string originalFilePath)
        {
            return this.GenerateFileName(Endpoint.GetFailurePath(), "err_", originalFilePath);
        }

        protected virtual void ProcessResultFile(FileInfo fileInfo, string destinationFileName, string logMessagePrefix, string originalFilePath)
        {
            try
            {
                this.CreateDirectories(destinationFileName);
                fileInfo.MoveTo(destinationFileName);

                Logger.Debug(string.Format("{0}: file {1} moved to {2}", logMessagePrefix, fileInfo.Name, destinationFileName));
                this.PostProcessor.PostProcess(destinationFileName, true);
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                var logPrefix = "FileNotFoundException processing " + fileInfo.Name;
                try
                {
                    TaskExtensions.Retry(() => File.Exists(this.GenerateSuccessFileName(originalFilePath)) || File.Exists(this.GenerateErrorFileName(originalFilePath)));
                    Logger.Debug(logPrefix + " output file located.");
                }
                catch (Exception)
                {
                    Logger.Error(logPrefix + " no success or failure file exists in output. Exception : " + fileNotFoundEx);
                }
            }
            catch (Exception exception)
            {
                Logger.Debug(string.Format("{0}: Exception {1}, Type: {2}", logMessagePrefix, exception.Message, exception.GetType()));
            }
        }


        protected abstract void StartCheckingForNewFiles();
        protected abstract void StopCheckingForNewFiles();
    }
}