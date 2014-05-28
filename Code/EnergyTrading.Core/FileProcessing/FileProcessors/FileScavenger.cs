namespace EnergyTrading.FileProcessing.FileProcessors
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;

    using EnergyTrading.FileProcessing.Configuration;
    using EnergyTrading.Logging;
    using EnergyTrading.Services;

    using Timer = System.Timers.Timer;

    public class FileScavenger : IStartable
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string NetworkNameNotFoundMsg = "The network name cannot be found";

        private FileProcessorEndpoint endpoint;
        private IFileFilter fileFilter;
        private string instanceId;
        private bool sufferedNetworkError;

        private readonly Timer timer;

        private Task currentTask;

        public event EventHandler<EventArgs> RestartFileChecking;

        public FileScavenger(FileProcessorEndpoint endpoint, IFileFilter fileFilter, string instanceId)
        {
            this.endpoint = endpoint;
            this.fileFilter = fileFilter;
            this.instanceId = instanceId;

            this.timer = new Timer(endpoint.ScavengeInterval.TotalMilliseconds) { AutoReset = true };
            this.timer.Elapsed += this.TimerElapsed;
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private void FireRestartEvent()
        {
            var fileChecking = RestartFileChecking;
            if (fileChecking != null)
            {
                fileChecking(this, new EventArgs());
            }
        }

        private static bool IsNetworkingError(Exception e)
        {
            return (e is IOException) ||
                   (string.Compare(e.Message, NetworkNameNotFoundMsg, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        protected void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            if (this.currentTask == null || this.currentTask.Status != TaskStatus.Running)
            {
                currentTask = Task.Factory.StartNew(this.ScavengerTask);
            }
        }

        private void ScavengeFiles(string enumeratePath, string dropPath)
        {
            // this is normally the case if inprogress path is not set
            if (string.IsNullOrWhiteSpace(enumeratePath))
            {
                return;
            }

            var fileDropExtension = this.endpoint.GetDropFilter().Substring(this.endpoint.GetDropFilter().LastIndexOf(".", StringComparison.InvariantCulture));
            var option = this.endpoint.MonitorSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            // TODO: Look at GetLastAccessTime since it might be stale
            var fileCount = 0;
            var movedCount = 0;
            foreach (var s in Directory.EnumerateFiles(enumeratePath, "*.*", option).Where(x => File.GetLastAccessTimeUtc(x) < (DateTime.UtcNow - this.endpoint.RecoveryInterval)))
            {
                ++fileCount;
                Logger.DebugFormat("Scavenger looking at {0}", s);
                try
                {
                    var source = s;
                    if ((s.EndsWith(fileDropExtension, StringComparison.InvariantCultureIgnoreCase) || (fileDropExtension == ".*" && !s.EndsWith(FileProcessorBase.InProgress, StringComparison.InvariantCultureIgnoreCase)))
                        && this.fileFilter.IncludeFile(s))
                    {
                        Logger.Debug("Processing file " + source);
                        var inprog = Path.ChangeExtension(s, "." + this.instanceId + "enqueue" + FileProcessorBase.InProgress);
                        // Atomic operation on all NTFS file systems - XP not included!
                        File.Move(s, inprog);
                        source = inprog;
                    }

                    if (source.EndsWith(FileProcessorBase.InProgress, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Logger.Debug("Reprocessing file " + source);
                        var withCorrectExtension = fileDropExtension == ".*" ? Path.ChangeExtension(source, ".txt") : Path.ChangeExtension(source, fileDropExtension);
                        // put back into drop path so that it gets picked up
                        var reprocessfilename = Path.Combine(dropPath, Path.GetFileName(withCorrectExtension));

                        File.Move(source, reprocessfilename);
                        ++movedCount;
                    }
                }
                catch (IOException ex)
                {
                    // we ignore FileNotFoundExceptions which could occur in File.Move because other threads are performing atomic operations
                    Logger.Debug("File not picked", ex);
                }

                // small sleep here prevents the loop from being too tight which seems to affect the file system response
                Thread.Sleep(100);
            }

            // if there was a network error or if we moved all of the files that we found then it indicates that the underlying item has stopped watching the folder and we 
            // fire the event to trigger a restart
            if (this.sufferedNetworkError || (fileCount > 0 && movedCount == fileCount))
            {
                this.FireRestartEvent();
            }
        }

        private void ScavengerTask()
        {
            Logger.Debug("Scavenging...");
            try
            {
                ScavengeFiles(endpoint.GetDropPath(), endpoint.GetDropPath()); // scavenge files stalled in drop path
                ScavengeFiles(endpoint.InProgressPath, endpoint.GetDropPath()); // scavenge flies stalled in progress path
            }
            catch (Exception exception)
            {
                if (IsNetworkingError(exception))
                {
                    this.sufferedNetworkError = true;
                }
                Logger.Debug(string.Format("Timer Exception {0}, Type: {1}", exception.Message, exception.GetType()));
            }
        }
    }
}