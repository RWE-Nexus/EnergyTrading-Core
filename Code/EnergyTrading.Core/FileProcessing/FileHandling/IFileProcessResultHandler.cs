namespace EnergyTrading.FileProcessing.FileHandling
{
    /// <summary>
    /// Handles events on files.
    /// </summary>
    public interface IFileProcessResultHandler
    {
        /// <summary>
        /// Handle an event on a file
        /// </summary>
        /// <param name="fileEvent">Event that has occurred</param>
        /// <param name="file">File affected</param>
        void Handle(FileProcessResult fileEvent, ProcessingFile file);
    }
}
