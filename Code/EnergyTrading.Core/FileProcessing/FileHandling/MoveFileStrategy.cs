namespace EnergyTrading.FileProcessing.FileHandling
{
    using System;

    [Obsolete("Use MoveFileHandlingStrategy")]
    public class MoveFileStrategy : MoveFileHandlingStrategy
    {
        public MoveFileStrategy(string targetDirectory) : base(targetDirectory)
        {
        }
    }
}