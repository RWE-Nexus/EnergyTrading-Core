namespace EnergyTrading.FileProcessing
{
    public class NullPostProcessor : IFilePostProcessor
    {
        public void PostProcess(string outputFile, bool successful)
        {
        }
    }
}
