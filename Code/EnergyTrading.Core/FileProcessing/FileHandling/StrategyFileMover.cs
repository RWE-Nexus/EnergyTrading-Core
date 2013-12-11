namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;

    [Obsolete("Use StrategyFileEventHandler")]
    public class StrategyFileMover : StrategyFileEventHandler
    {
        public StrategyFileMover(IFileHandlingStrategy successStrategy, IFileHandlingStrategy cancelledStrategy, IFileHandlingStrategy failureStrategy)
            : base(successStrategy, cancelledStrategy, failureStrategy)
        {
        }
    }
}