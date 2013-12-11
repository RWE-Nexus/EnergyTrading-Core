namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;

    /// <summary>
    /// Strategy that applies multiple strategies to the file,
    /// </summary>
    public class CombinedFileHandlingStrategy : IFileHandlingStrategy
    {
        private readonly IFileHandlingStrategy[] strategies;

        /// <summary>
        /// Create a new instance of the <see cref="CombinedFileHandlingStrategy" /> class.
        /// </summary>
        /// <param name="strategies">Strategies to use.</param>
        public CombinedFileHandlingStrategy(params IFileHandlingStrategy[] strategies)
        {
            if (strategies == null)
            {
                throw new ArgumentNullException("strategies");
            }
            if (strategies.Length == 0)
            {
                throw new ArgumentException("Must supply at least one strategy");
            }

            this.strategies = strategies;
        }

        /// <summary>
        /// Apply the strategies to the file.
        /// </summary>
        /// <param name="processingFile">File to use</param>
        public void Handle(ProcessingFile processingFile)
        {
            foreach (var strategy in strategies)
            {
                strategy.Handle(processingFile);
            }
        }
    }
}