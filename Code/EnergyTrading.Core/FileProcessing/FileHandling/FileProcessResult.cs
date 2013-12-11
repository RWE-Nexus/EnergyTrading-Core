namespace EnergyTrading.FileProcessing.FileHandling
{
    /// <summary>
    /// Events that can have to files
    /// </summary>
    public enum FileProcessResult
    {
        /// <summary>
        /// File has been successfully processed.
        /// </summary>
        Processed = 0,

        /// <summary>
        /// File processing has been cancelled.
        /// </summary>
        Cancelled = 1,

        /// <summary>
        /// An error occurred during processing.
        /// </summary>
        Error = 2
    }
}