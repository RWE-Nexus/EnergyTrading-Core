namespace EnergyTrading.FileProcessing
{
    public class DefaultFileFilter : IFileFilter
    {
        public bool IncludeFile(string fullFilePath)
        {
            return true;
        }
    }
}