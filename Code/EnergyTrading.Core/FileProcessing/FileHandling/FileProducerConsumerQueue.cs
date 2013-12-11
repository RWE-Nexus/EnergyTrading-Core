namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using EnergyTrading.Logging;
    using EnergyTrading.ProducerConsumer;

    public class FileProducerConsumerQueue : ProducerConsumerQueueBase, IHandleFiles
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFileProcessResultHandler processResultHandler;
        private readonly IFileHandler fileProcessor;

        public FileProducerConsumerQueue(int numberOfConsumers, IFileProcessResultHandler processResultHandler, IFileHandler fileProcessor)
            : base(numberOfConsumers)
        {
            if (processResultHandler == null) { throw new ArgumentNullException("processResultHandler"); }
            if (fileProcessor == null) { throw new ArgumentNullException("fileProcessor"); }

            this.processResultHandler = processResultHandler;
            this.fileProcessor = fileProcessor;
        }

        public void Notify(ProcessingFile processingFile)
        {
            ThrowIfDisposed();

            var fileProcessorFullPath = fileProcessor as IFileHandlerFullPath;
            Task task;
            task = fileProcessorFullPath != null ? 
                this.EnqueueWork(() => fileProcessorFullPath.HandleWithFullPath(new FileInfo(processingFile.CurrentFilePath), processingFile.FullPathOfOriginalFile)) : 
                this.EnqueueWork(() => this.fileProcessor.Handle(new FileInfo(processingFile.CurrentFilePath), processingFile.OriginalFilePath));
            task.ContinueWith(x => FileProcessed(processingFile), TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
            task.ContinueWith(x => FileCancelled(processingFile), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.ExecuteSynchronously);
            task.ContinueWith(x => FileErrored(processingFile, x.Exception), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        private void FileProcessed(ProcessingFile processingFile)
        {
            Logger.Debug(string.Format("File processed successfully: {0}", processingFile.OriginalFilePath));
            processResultHandler.Handle(FileProcessResult.Processed, processingFile);
        }

        private void FileCancelled(ProcessingFile processingFile)
        {
            Logger.Debug(string.Format("File processing cancelled: {0}", processingFile.OriginalFilePath));
            processResultHandler.Handle(FileProcessResult.Cancelled, processingFile);
        }

        private void FileErrored(ProcessingFile processingFile, AggregateException exception)
        {
            Logger.Error(string.Format("File processing failed: {0}", processingFile.OriginalFilePath), exception.Flatten());
            processResultHandler.Handle(FileProcessResult.Error, processingFile);
        }
    }
}