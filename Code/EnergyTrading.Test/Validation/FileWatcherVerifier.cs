namespace EnergyTrading.Test.Validation
{
    using System;
    using System.IO;

    /// <summary>
    /// Verification for a file watcher
    /// </summary>
    public class FileWatcherVerifier : IVerifier
    {
        public string MonitorFolderPath { get; private set; }
        public string Filter { get; private set; }
        private FileSystemWatcher fileSystemWatcher;

        public event EventHandler<EventArgs> VerificationPerformed = delegate { };

        public FileWatcherVerifier(string monitorFolderPath, string filter)
        {
            this.MonitorFolderPath = monitorFolderPath;
            this.Filter = filter;
        }

        public void Start()
        {
            this.fileSystemWatcher = new FileSystemWatcher(this.MonitorFolderPath, this.Filter);
            this.fileSystemWatcher.Created += this.NewFileReceived;
            this.fileSystemWatcher.IncludeSubdirectories = false;
            this.fileSystemWatcher.EnableRaisingEvents = true;            
        }

        public void Stop()
        {
            if (this.fileSystemWatcher != null)
            {
                this.fileSystemWatcher.EnableRaisingEvents = false;
            }
        }

        private void NewFileReceived(object sender, FileSystemEventArgs args)
        {
            // determine comparison file path
            var comparisonFile = this.DetermineComparisonFileNameFromReceived(args.Name);

            // perform comparison

            // construct result

            // fire event
        }

        private string DetermineComparisonFileNameFromReceived(string name)
        {
            throw new NotImplementedException();
        }
    }
}