namespace EnergyTrading.Wrappers
{
    using System.IO;

    using EnergyTrading.Attributes;

    [ExcludeFromCoverage]
    public class FileWrapper : IFile
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void WriteAllText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        public void Move(string sourceFilePath, string destFilePath)
        {
            File.Move(sourceFilePath, destFilePath);
        }

        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }

        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public FileStream Open(string filePath, FileMode mode)
        {
            return File.Open(filePath, mode);
        }

        public FileStream Open(string filePath, FileMode mode, FileAccess access)
        {
            return File.Open(filePath, mode, access);
        }

        public FileStream Open(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            return File.Open(filePath, mode, access, share);
        }
    }
}