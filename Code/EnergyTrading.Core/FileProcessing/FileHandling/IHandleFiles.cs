namespace EnergyTrading.FileProcessing.FileHandling
{
    /// <summary>
    /// Process a file
    /// </summary>
    public interface IHandleFiles
    {
        /// <summary>
        /// Notify that a file is ready for processing.
        /// </summary>
        /// <param name="processingFile"></param>
        void Notify(ProcessingFile processingFile);
    }
}