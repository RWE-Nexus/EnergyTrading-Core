namespace EnergyTrading.UnitTest.FileProcessing
{
    using EnergyTrading.FileProcessing;

    public class FileProcessorHelper : FileProcessor
    {
        public FileProcessorHelper(FileProcessorEndpoint endpoint, IFileHandler handler, IFilePostProcessor postProcessor)
            : base(endpoint, handler, postProcessor)
        {
        }

        public new string GenerateSuccessFileName(string originalpath)
        {
            return base.GenerateSuccessFileName(originalpath);
        }

        public new string GenerateErrorFileName(string originalpath)
        {
            return base.GenerateErrorFileName(originalpath);
        }
    }
}
