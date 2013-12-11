namespace EnergyTrading.Wrappers
{
    using System.IO;

    /// <summary>
    /// Injection interface to allow for mocking when performing tasks against the file system
    /// </summary>
    public interface IDirectory
    {
        bool Exists(string path);
        DirectoryInfo CreateDirectory(string path);
        void Delete(string path);
    }
}