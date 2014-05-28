namespace EnergyTrading.FileProcessing.FileProcessors
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Timers;

    using EnergyTrading.FileProcessing.Configuration;
    using EnergyTrading.Logging;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// The polling basedv 2 file processor.
    /// </summary>
    public class PollingBasedv2FileProcessor : FileProcessorBase
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Timer pollingTimer;
        private Task currentTask;

        public PollingBasedv2FileProcessor(FileProcessorEndpoint endpoint, IFileHandler handler, IFilePostProcessor postProcessor, IFileFilter additionalFilter)
            : base(endpoint, handler, postProcessor, additionalFilter)
        {
            this.pollingTimer = new Timer(Endpoint.PollingInterval) { AutoReset = true };
            this.pollingTimer.Elapsed += this.TimerElapsed;
        }

        protected override void StartCheckingForNewFiles()
        {
            // run a poll immediately to pick up any waiting files and then start the timer 
            this.TimerElapsed(this, new ElapsedEventArgs());
            this.pollingTimer.Start();
            Logger.DebugFormat(
               "Now watching for any new files dropped at {0}{1} with filter {2}",
               this.Endpoint.GetDropPath(),
               this.Endpoint.MonitorSubdirectories ? " and subdirectories" : string.Empty,
               this.Endpoint.GetDropFilter());
        }

        protected override void StopCheckingForNewFiles()
        {
            Logger.Debug("Stopping File Polling Timer");
            this.pollingTimer.Stop();
        }

        protected void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            if (this.currentTask == null || this.currentTask.Status != TaskStatus.Running)
            {
                currentTask = Task.Factory.StartNew(this.PollingTask);
            }
        }

        private void PollingTask()
        {
            try
            {
                var fileDropExtension = this.Endpoint.GetDropFilter().Substring(this.Endpoint.GetDropFilter().LastIndexOf(".", StringComparison.InvariantCulture));
                var option = this.Endpoint.MonitorSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                foreach (var file in Directory.EnumerateFiles(Endpoint.GetDropPath(), "*.*", option))
                {
                    if (!file.EndsWith(fileDropExtension, StringComparison.InvariantCultureIgnoreCase) && (fileDropExtension != ".*"))
                    {
                        continue;
                    }

                    Logger.Debug("Notified: " + file);

                    // ignore notifications for inprogress extensions
                    if (file.EndsWith(InProgress))
                    {
                        Logger.Debug("Ignoring: " + file);
                        continue;
                    }

                    if (!this.AdditionalFilter.IncludeFile(file))
                    {
                        Logger.Debug("Skipping file excluded by additional Filter " + file);
                        return;
                    }
                    this.AddFileToQueue(file);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while polling directory for files", ex);
            }
        }

    }
}