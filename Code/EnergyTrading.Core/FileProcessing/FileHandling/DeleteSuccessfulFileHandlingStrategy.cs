namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    using EnergyTrading.FileProcessing.Exceptions;
    using EnergyTrading.Logging;

    /// <summary>
    /// Strategy to delete files that have been processed successfully.
    /// </summary>
    public class DeleteSuccessfulFileHandlingStrategy: IFileHandlingStrategy
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Delete a file that has been processed successfully.
        /// </summary>
        /// <param name="processingFile">File to process</param>
        public void Handle(ProcessingFile processingFile)
        {
            var file = new FileInfo(processingFile.CurrentFilePath);
            if (file.Exists)
            {
                file.Delete();
            }

            const int MaxTries = 10;
            var tries = 0;
            Exception lastException;
            do
            {
                tries++;
                try
                {
                    File.Delete(processingFile.CurrentFilePath);
                    Logger.InfoFormat("Deleted: {0}", processingFile.CurrentFilePath);
                    return;
                }
                catch (IOException ex)
                {
                    lastException = ex;

                    Logger.InfoFormat(
                        "Failed deleting {0} - attempt {1}/{2}", processingFile.CurrentFilePath, tries, MaxTries);
                    Thread.Sleep(1000);
                }
            }
            while (tries < MaxTries);

            throw new FileDeleteException(processingFile.CurrentFilePath, lastException);
        }
    }
}