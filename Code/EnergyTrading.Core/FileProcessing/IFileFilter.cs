namespace EnergyTrading.FileProcessing
{
    public interface IFileFilter
    {
        bool IncludeFile(string fullFilePath);
    }
}