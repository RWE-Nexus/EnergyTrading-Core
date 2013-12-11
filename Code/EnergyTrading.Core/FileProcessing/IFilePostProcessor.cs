namespace EnergyTrading.FileProcessing
{
    public interface IFilePostProcessor
    {
        void PostProcess(string outputFile, bool successful);
    }
}
