namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;
    using System.IO;

    /// <summary>
    /// Handles files by passing off to appropriate <see cref="IFileHandlingStrategy" /> instances.
    /// </summary>
    public class StrategyFileEventHandler : IFileProcessResultHandler, IMoveFiles
    {
        private readonly IFileHandlingStrategy successStrategy;
        private readonly IFileHandlingStrategy cancelledStrategy;
        private readonly IFileHandlingStrategy failureStrategy;

        /// <summary>
        /// Create a new instance of the <see cref="StrategyFileEventHandler" /> class.
        /// </summary>
        /// <param name="successStrategy"></param>
        /// <param name="cancelledStrategy"></param>
        /// <param name="failureStrategy"></param>
        public StrategyFileEventHandler(IFileHandlingStrategy successStrategy, IFileHandlingStrategy cancelledStrategy, IFileHandlingStrategy failureStrategy)
        {
            if (successStrategy == null)
            {
                throw new ArgumentNullException("successStrategy");
            }
            if (cancelledStrategy == null)
            {
                throw new ArgumentNullException("cancelledStrategy");
            }
            if (failureStrategy == null)
            {
                throw new ArgumentNullException("failureStrategy");
            }

            this.successStrategy = successStrategy;
            this.cancelledStrategy = cancelledStrategy;
            this.failureStrategy = failureStrategy;
        }

        [Obsolete("Use Handle")]
        public void FileErrored(ProcessingFile processingFile)
        {
            Handle(FileProcessResult.Error, processingFile);
        }

        [Obsolete("Use Handle")]
        public void FileCancelled(ProcessingFile processingFile)
        {
            Handle(FileProcessResult.Cancelled, processingFile);
        }

        [Obsolete("Use Handle")]
        public void FileProcessed(ProcessingFile processingFile)
        {
            Handle(FileProcessResult.Processed, processingFile);
        }

        /// <copydocfrom cref="IFileProcessResultHandler.Handle" />
        public void Handle(FileProcessResult fileEvent, ProcessingFile file)
        {
            if (!File.Exists(file.CurrentFilePath))
            {
                // TODO: Log
                return;
            }
            
            switch (fileEvent)
            {
                case FileProcessResult.Cancelled:
                    cancelledStrategy.Handle(file);
                    break;
                
                case FileProcessResult.Error:
                    failureStrategy.Handle(file);
                    break;

                case FileProcessResult.Processed:                    
                    successStrategy.Handle(file);
                    break;

                default:
                    throw new NotSupportedException("Unknown event: " + fileEvent);
            }
        }
    }
}