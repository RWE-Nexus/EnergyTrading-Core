namespace EnergyTrading.FileProcessing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using EnergyTrading.Configuration;

    public class IgnoreDirectoriesFileFilter : IFileFilter
    {
        private readonly bool matchCase;
        private readonly List<string> directoriesToExclude = new List<string>();

        public IgnoreDirectoriesFileFilter(IConfigurationManager configuration)
        {
            matchCase = string.Equals("true", configuration.GetAppSettingValue("IgnoreDirectoriesFilterMatchCase"), StringComparison.InvariantCultureIgnoreCase);
            var directories = configuration.GetAppSettingValue("IgnoreDirectoriesFilterList");
            if (!string.IsNullOrEmpty(directories))
            {
                this.directoriesToExclude.AddRange(directories.Split(','));
            }
        }

        public bool IncludeFile(string fullFilePath)
        {
            return this.GetNonRootDirectoriesInFilePath(fullFilePath).All(dir => this.directoriesToExclude.Find(s => string.Equals(s, dir, this.matchCase ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase)) == null);
        }

        protected IEnumerable<string> GetNonRootDirectoriesInFilePath(string fullPath)
        {
            var directoryPath = Path.GetDirectoryName(fullPath);
            while (directoryPath != null && (!Path.IsPathRooted(fullPath) || !string.Equals(directoryPath, Path.GetPathRoot(fullPath))))
            {
                yield return Path.GetFileName(directoryPath);
                directoryPath = Path.GetDirectoryName(directoryPath);
            }
        }
    }
}