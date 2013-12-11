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
            MonitorFolderPath = monitorFolderPath;
            Filter = filter;
        }

        public void Start()
        {
            fileSystemWatcher = new FileSystemWatcher(MonitorFolderPath, Filter);
            fileSystemWatcher.Created += NewFileReceived;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;            
        }

        public void Stop()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
            }
        }

        private void NewFileReceived(object sender, FileSystemEventArgs args)
        {
            // determine comparison file path
            var comparisonFile = DetermineComparisonFileNameFromReceived(args.Name);

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