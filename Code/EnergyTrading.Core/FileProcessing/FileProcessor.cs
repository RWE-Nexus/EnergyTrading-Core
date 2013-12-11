namespace EnergyTrading.FileProcessing
{
    using System;
    using System.Collections.Concurrent;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Timers;

    using EnergyTrading.Logging;
    using EnergyTrading.Threading;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// Provides a processor that listens for files and then passes them onto a handler.
    /// <para>
    /// Caters for issues such as IO and network errors and if operating against NTFS performs atomic operations.
    /// </para>
    /// </summary>
    public class FileProcessor : IFileProcessor
    {
        private const string InProgress = "inprogress";
        private const string NetworkNameNotFoundMsg = "The network name cannot be found";

        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Timer timer;
        private readonly ConcurrentQueue<string> pendingFiles;
        private readonly AutoResetEvent eventHandle;
        private readonly ManualResetEvent quitHandle;
        private Thread processingThread;
        private bool quitting;
        private Guid instanceId;
        private FileSystemWatcher watcher;
        private bool sufferedNetworkError;

        public FileProcessor(FileProcessorEndpoint endpoint, IFileHandler handler, IFilePostProcessor postProcessor)
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

            this.instanceId = Guid.Empty;
            this.Endpoint = endpoint;
            this.Handler = handler;
            this.PostProcessor = postProcessor;

            this.timer = new Timer(this.Endpoint.ScavengeInterval.TotalMilliseconds) { AutoReset = true };
            this.timer.Elapsed += this.TimerElapsed;

            this.pendingFiles = new ConcurrentQueue<string>();
            this.eventHandle = new AutoResetEvent(false);
            this.quitHandle = new ManualResetEvent(false);
            this.quitting = false;

            Logger.DebugFormat("Scavenger interval set at {0:0.0} seconds", endpoint.ScavengeInterval.TotalSeconds);
        }

        public string Name 
        {
            get { return this.Endpoint.Name; }
        }

        protected FileSystemWatcher Watcher
        {
            get { return watcher; }
        }

        protected virtual string InstanceId
        {
            get
            {
                if (instanceId == Guid.Empty)
                {
                    instanceId = Guid.NewGuid();
                }
                return instanceId.ToString().Replace("-", string.Empty);
            }
        }

        protected virtual string DropPath
        {
            get
            {
                return this.GetConfigPropertyOrThrow(() => this.Endpoint.DropPath, "File Drop Path");
            }
        }

        protected virtual string DropFilter
        {
            get
            {
                return this.GetConfigPropertyOrThrow(() => this.Endpoint.Filter, "File Drop Filter");
            }
        }

        protected virtual string SuccessPath
        {
            get
            {
                return this.GetConfigPropertyOrThrow(() => this.Endpoint.SuccessPath, "File Success Path");
            }
        }

        protected virtual string FailurePath
        {
            get
            {
                return this.GetConfigPropertyOrThrow(() => this.Endpoint.FailurePath, "File Failure Path");
            }
        }

        private FileProcessorEndpoint Endpoint { get; set; }

        private IFileHandler Handler { get; set; }

        private IFilePostProcessor PostProcessor { get; set; }

        public void Start()
        {
            this.Stop();
            Logger.Debug("Starting");

            quitting = false;
            quitHandle.Reset();

            this.StartFileWatcher();
            this.timer.Start();

            processingThread = new Thread(this.ProcessFiles);
            processingThread.Start();

            Logger.Debug("Started");
        }

        public void Stop()
        {
            Logger.Debug("Stopping");
            this.StopFileWatcher();
            this.timer.Stop();

            quitting = true;
            if (processingThread != null)
            {
                // wait till we receive signal from ProcessingFiles
                // TODO: Might want this configurable - Endpoint
                quitHandle.WaitOne(TimeSpan.FromSeconds(10));

                processingThread = null;
            }
            Logger.Debug("Stopped");
        }

        protected void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            Logger.Debug("Scavenging...");
            try
            {
                var fileDropExtension = this.DropFilter.Substring(this.DropFilter.LastIndexOf(".", StringComparison.InvariantCulture));
                var option = Endpoint.MonitorSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                // TODO: Look at GetLastAccessTime since it might be stale
                foreach (var s in Directory.EnumerateFiles(this.DropPath, "*.*", option).Where(x => File.GetLastAccessTimeUtc(x) < (DateTime.UtcNow - this.Endpoint.RecoveryInterval)))
                {
                    Logger.DebugFormat("Scavenger looking at {0}", s);
                    try
                    {
                        var source = s;
                        if (s.EndsWith(fileDropExtension, StringComparison.InvariantCultureIgnoreCase) || (fileDropExtension == ".*" && !s.EndsWith(InProgress, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            Logger.Debug("Processing file " + source);
                            var inprog = Path.ChangeExtension(s, "." + InstanceId + "enqueue" + InProgress);
                            // Atomic operation on all NTFS file systems - XP not included!
                            File.Move(s, inprog);
                            source = inprog;
                        }

                        if (source.EndsWith(InProgress, StringComparison.InvariantCultureIgnoreCase))
                        {
                            Logger.Debug("Reprocessing file " + source);
                            var reprocessfilename = fileDropExtension == ".*" ? Path.ChangeExtension(source, ".txt") : Path.ChangeExtension(source, fileDropExtension);
                            File.Move(source, reprocessfilename);
                        }
                    }
                    catch (System.IO.IOException ex)
                    {
                        // we ignore FileNotFoundExceptions which could occur in File.Move because other threads are performing atomic operations
                        Logger.Debug("File not picked", ex);
                    }

                    // small sleep here prevents the loop from being too tight which seems to affect the file system response
                    Thread.Sleep(100);
                }

                if (sufferedNetworkError)
                {
                    StopFileWatcher();
                    StartFileWatcher();
                    sufferedNetworkError = false;
                }
            }
            catch (Exception exception)
            {
                if (IsNetworkingError(exception))
                {
                    sufferedNetworkError = true;
                }
                Logger.Debug(string.Format("Timer Exception {0}, Type: {1}", exception.Message, exception.GetType()));
            }
        }

        protected virtual void ProcessResultFile(FileInfo fileInfo, string destinationFileName, string logMessagePrefix, string originalFilePath)
        {
            try
            {
                this.CreateDirectories(destinationFileName);
                fileInfo.MoveTo(destinationFileName);

                Logger.Debug(string.Format("{0}: file {1} moved to {2}", logMessagePrefix, fileInfo.Name, destinationFileName));
                PostProcessor.PostProcess(destinationFileName, true);
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                var logPrefix = "FileNotFoundException processing " + fileInfo.Name;
                try
                {
                    TaskExtensions.Retry(() => File.Exists(GenerateSuccessFileName(originalFilePath)) || File.Exists(GenerateErrorFileName(originalFilePath)));
                    Logger.Debug(logPrefix + " output file located.");
                }
                catch(Exception)
                {
                    Logger.Error(logPrefix + " no success or failure file exists in output. Exception : " + fileNotFoundEx);
                }
            }
            catch (Exception exception)
            {
                Logger.Debug(string.Format("{0}: Exception {1}, Type: {2}", logMessagePrefix, exception.Message, exception.GetType()));
            }
        }

        /// <summary>
        /// Logic for processing a successful file
        /// </summary>
        /// <param name="fileInfo">File info of the successful file.</param>
        /// <param name="originalFilePath"></param>
        protected virtual void ProcessSuccessfulFile(FileInfo fileInfo, string originalFilePath)
        {
            ProcessResultFile(fileInfo, GenerateSuccessFileName(originalFilePath), "File Process Success", originalFilePath);
        }

        /// <summary>
        /// Logic for processing a failed file
        /// </summary>
        protected virtual void ProcessFailedFile(FileInfo fileInfo, string originalFilePath)
        {
            ProcessResultFile(fileInfo, GenerateErrorFileName(originalFilePath), "File Process Failure", originalFilePath);
        }

        protected void CreateDirectories(string filepath)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(filepath));
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        protected virtual string GenerateSuccessFileName(string originalFilePath)
        {
            var successDir = SuccessPath;
            var dirpath = GetPathWithoutWatchedFolder(originalFilePath);
            if (SuccessPath.Contains("%filepath%"))
            {
                successDir = SuccessPath.Replace("%filepath%", Path.GetDirectoryName(originalFilePath));
                dirpath = Path.GetFileName(originalFilePath);
            }
            var errorFileName = dirpath + ".suc_" + DateTime.Now.Ticks;
            return Path.Combine(successDir, errorFileName);
        }

        protected virtual string GenerateErrorFileName(string originalFilePath)
        {
            var failDir = FailurePath;
            var dirpath = GetPathWithoutWatchedFolder(originalFilePath);
            if (FailurePath.Contains("%filepath%"))
            {
                failDir = FailurePath.Replace("%filepath%", Path.GetDirectoryName(originalFilePath));
                dirpath = Path.GetFileName(originalFilePath);
            }
            var errorFileName = dirpath + ".err_" + DateTime.Now.Ticks;
            return Path.Combine(failDir, errorFileName);
        }

        protected virtual string GetConfigPropertyOrThrow(Func<string> propertyGetter, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyGetter()))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} has not been configured", propertyName));
            }

            return propertyGetter().Trim();
        }

        private static bool IsFileLockedError(Exception exception)
        {
            var code = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return code == 32 || code == 33;
        }

        private static bool IsNetworkingError(Exception e)
        {
            return (e is IOException) ||
                   (string.Compare(e.Message, NetworkNameNotFoundMsg, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        private string GetPathWithoutWatchedFolder(string path)
        {
            return path.Substring(DropPath.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) ? DropPath.Length : DropPath.Length + 1);
        }

        private void StartFileWatcher()
        {
            this.watcher = new FileSystemWatcher(this.DropPath, this.DropFilter);
            this.watcher.InternalBufferSize = 4 * 4096; //1024768;  // Try to avoid missing events
            this.watcher.Created += this.NewFileNotification;
            this.watcher.Renamed += this.NewFileNotification;
            this.watcher.IncludeSubdirectories = this.Endpoint.MonitorSubdirectories;
            this.watcher.EnableRaisingEvents = true;

            Logger.DebugFormat(
                "Now watching for any new files dropped at {0}{1} with filter {2}",
                this.DropPath,
                this.Endpoint.MonitorSubdirectories ? " and subdirectories" : string.Empty,
                this.DropFilter);
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

                Logger.Debug("Enqueing: " + fileName);
                pendingFiles.Enqueue(e.FullPath);
                eventHandle.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ProcessFile : " + ex.Message);
            }
        }

        private void ProcessFiles()
        {
            while (!quitting)
            {
                // Sleep for a while or until we get a task.
                eventHandle.WaitOne(TimeSpan.FromSeconds(5));

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
                        if (this.HandleFile(info, Path.GetFileName(originalFilePath)))
                        {
                            this.ProcessSuccessfulFile(info, originalFilePath);
                        }
                        else
                        {
                            this.ProcessFailedFile(info, originalFilePath);
                        }
                    }

                    if (quitting)
                    {
                        // Exit soon as we know.
                        break;
                    }
                }
            }

            quitHandle.Set();
        }
        
        private bool HandleFile(FileInfo fileInfo, string originalFileName)
        {
            Logger.Debug("Handling: " + fileInfo);
            try
            {
                return this.Handler.Handle(fileInfo, originalFileName);
            }
            catch (Exception e)
            {
                Logger.Error("Unhandled error in file handler", e);
                return false;
            }
        }

        private bool TakePossessionOfFileForProcessing(string sourcePath, out string processPath)
        {
            Logger.Debug("Acquiring: " + Path.GetFileName(sourcePath));

            processPath = string.Empty;
            var inprogressFile = Path.ChangeExtension(sourcePath, "." + InstanceId + InProgress);
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
    }
}