namespace EnergyTrading.FileProcessing
{
    using System.IO;

    /// <summary>
    /// Extends IFileHandler with a function that handles an inprogress file and supplies the full path of the original file
    /// When a file is being processed if the supplied IFileHandler implements this interface HandleWithFullPath will be called instead of IFileHandler.Handle
    /// </summary>
    public interface IFileHandlerFullPath : IFileHandler
    {
        /// <summary>
        /// Process a file returning whether we were successful or not.
        /// </summary>
        /// <param name="fileInfo">File to process</param>
        /// <param name="originalFileFullPath">Original full path to dropped file</param>
        /// <returns>true if successful, false otherwise.</returns>
        bool HandleWithFullPath(FileInfo fileInfo, string originalFileFullPath);
    }
}