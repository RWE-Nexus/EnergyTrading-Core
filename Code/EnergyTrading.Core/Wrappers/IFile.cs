namespace EnergyTrading.Wrappers
{
    using System.IO;

    /// <summary>
    /// Injection interface to allow for mocking when performing tasks against the file system
    /// </summary>
    public interface IFile
    {
        bool Exists(string filePath);
        void WriteAllText(string filePath, string content);
        void Move(string sourceFilePath, string destFilePath);
        void Delete(string filePath);
        string ReadAllText(string filePath);
        FileStream Open(string filePath, FileMode mode);
        FileStream Open(string filePath, FileMode mode, FileAccess access);
        FileStream Open(string filePath, FileMode mode, FileAccess access, FileShare share);
    }
}