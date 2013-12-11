namespace EnergyTrading.FileProcessing
{
    using System.IO;

    /// <summary>
    /// Handles files for a business process.
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Process a file returning whether we were successful or not.
        /// </summary>
        /// <param name="fileInfo">File to process</param>
        /// <param name="originalFileName">Original name of dropped file</param>
        /// <returns>true if successful, false otherwise.</returns>
        bool Handle(FileInfo fileInfo, string originalFileName);
    }
}