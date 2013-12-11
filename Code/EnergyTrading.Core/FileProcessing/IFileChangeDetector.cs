namespace EnergyTrading.FileProcessing
{
    using System.IO;

    using EnergyTrading.Services;

    /// <summary>
    /// Detects changes to the file system.
    /// </summary>
    public interface IFileChangeDetector : IStartable
    {
        /// <summary>
        /// Raised on any type of file change - new, renamed
        /// </summary>
        event FileSystemEventHandler Changed;

        /// <summary>
        /// Gets or sets the path to monitor.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets or sets the filter to use.
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Gets or sets whether we monitor subdirectories.
        /// </summary>
        bool MonitorSubdirectories { get; set; }
    }
}