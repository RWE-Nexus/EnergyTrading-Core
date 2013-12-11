namespace EnergyTrading.FileProcessing.FileHandling
{
    /// <summary>
    /// Strategy for handling files under various scenarios, e.g. success, failure, deletion etc.
    /// </summary>
    public interface IFileHandlingStrategy
    {
        /// <summary>
        /// Handle the file.
        /// </summary>
        /// <param name="processingFile">File to handle</param>
        void Handle(ProcessingFile processingFile);
    }
}