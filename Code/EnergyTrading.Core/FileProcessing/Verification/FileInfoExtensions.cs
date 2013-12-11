namespace EnergyTrading.FileProcessing.Verification
{
    using System.IO;

    using EnergyTrading.Configuration;

    public static class FileInfoExtensions
    {
        public static bool IsTestFile(this FileInfo fileInfo, IConfigurationManager configurationManager)
        {
            if (fileInfo == null)
            {
                return false;
            }
            var prefix = configurationManager.GetVerificationPrefix();
            return fileInfo.Name.StartsWith(prefix);
        }
    }
}