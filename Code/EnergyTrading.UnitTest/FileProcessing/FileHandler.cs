namespace EnergyTrading.UnitTest.FileProcessing
{
    using System.IO;

    using EnergyTrading.FileProcessing;

    public class FileHandler : IFileHandler
    {
        public bool Handle(FileInfo fileInfo, string originalFileName)
        {
            return true;
        }
    }
}