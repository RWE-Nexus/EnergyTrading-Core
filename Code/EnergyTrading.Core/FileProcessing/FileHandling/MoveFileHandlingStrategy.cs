namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    using EnergyTrading.Logging;

    /// <summary>
    /// Strategy that moves files to a target directory.
    /// </summary>
    public class MoveFileHandlingStrategy : IFileHandlingStrategy
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const int MaxTries = 3;
        private readonly string targetDirectory;

        /// <summary>
        /// Creates a new instance of the <see cref="MoveFileHandlingStrategy" />
        /// </summary>
        /// <param name="targetDirectory">Directory to move files into</param>
        public MoveFileHandlingStrategy(string targetDirectory)
        {
            this.targetDirectory = targetDirectory;
        }

        /// <summary>
        /// Move a file into a directory.
        /// </summary>
        /// <param name="processingFile">File to move</param>
        public void Handle(ProcessingFile processingFile)
        {
            var destinationDir = targetDirectory;
            if (targetDirectory.Contains("%filepath%"))
            {
                destinationDir = targetDirectory.Replace("%filepath%", Path.GetDirectoryName(processingFile.FullPathOfOriginalFile));
            }
            Logger.DebugFormat("Moving {0} to {1}", processingFile.OriginalFilePath, destinationDir);
            string newFilePath = Path.Combine(destinationDir, processingFile.OriginalFilePath);

            var tries = 0;
            Exception lastException;
            do
            {
                tries++;
                try
                {
                    File.Move(processingFile.CurrentFilePath, newFilePath);
                    Logger.InfoFormat("{0} moved to {1}", processingFile.OriginalFilePath, destinationDir);
                    return;
                }
                catch (IOException ex)
                {
                    lastException = ex;

                    if (ex.Message.ToLowerInvariant().Contains("already exists"))
                    {
                        Logger.Warn(string.Format("Unable to move file to {0}", newFilePath), ex);
                        return;
                    }

                    Logger.DebugFormat(
                        "Failed moving {0} to {1} - attempt {2}/{3}",
                        processingFile.OriginalFilePath,
                        destinationDir,
                        tries,
                        MaxTries);
                    Thread.Sleep(500);
                }
            }
            while (tries < MaxTries);

            Logger.Warn(string.Format("Failed moving {0} to {1}", processingFile.OriginalFilePath, destinationDir), lastException);
        }
    }
}