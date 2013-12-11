namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;

    /// <summary>
    /// Metadata about a file we are processing.
    /// </summary>
    public class ProcessingFile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ProcessingFile" /> class.
        /// </summary>
        /// <param name="currentFilePath">Current location of the file</param>
        /// <param name="originalFilePath">Original location of the file</param>
        /// <param name="originalFullPathToFile">Original location of the file FileInfo.FullName</param>
        public ProcessingFile(string currentFilePath, string originalFilePath, string originalFullPathToFile)
        {
            CurrentFilePath = currentFilePath;
            OriginalFilePath = originalFilePath;
            FullPathOfOriginalFile = originalFullPathToFile;
        }

        [Obsolete("Use CurrentFilePath")]
        public string CurrentLocation
        {
            get { return CurrentFilePath; }
        }

        [Obsolete("Use OriginalFilePath")]
        public string OriginalFilename
        {
            get { return OriginalFilePath; }
        }

        /// <summary>
        /// Gets the current location of the file.
        /// </summary>
        public string CurrentFilePath { get; private set; }

        /// <summary>
        /// Gets the original name of the file
        /// </summary>
        public string OriginalFilePath { get; private set; }

        /// <summary>
        /// Gets the full path of the original file FileInfo.FullName
        /// </summary>
        public string FullPathOfOriginalFile { get; private set; }
    }
}