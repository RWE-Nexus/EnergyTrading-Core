namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;

    [Obsolete("Use IFileEventHandler")]
    public interface IMoveFiles
    {
        [Obsolete("Use IFileEventHandler.Handle")]
        void FileErrored(ProcessingFile processingFile);
            
        [Obsolete("Use IFileEventHandler.Handle")]
        void FileCancelled(ProcessingFile processingFile);

        [Obsolete("Use IFileEventHandler.Handle")]
        void FileProcessed(ProcessingFile processingFile);
    }
}