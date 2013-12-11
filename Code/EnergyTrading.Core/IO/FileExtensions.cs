namespace EnergyTrading.IO
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web;

    using EnergyTrading.Logging;

    /// <summary>
    /// Extension methods for operating with files/paths
    /// </summary>
    public static class FileExtensions
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Handles web relative file names
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string MapPath(this string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.StartsWith("~"))
                {
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Server.MapPath(fileName);
                    }

                    throw new NotSupportedException("Cannot map web paths without a web context");
                }

                if (fileName.StartsWith("."))
                {
                    // Convert to a full path
                    return Path.GetFullPath(fileName);
                }
            }

            // Return the one we are given
            return fileName;
        }

        /// <summary>
        /// Delete files in a path, handling wildcards
        /// </summary>
        /// <param name="searchPattern"></param>
        public static void DeleteFiles(this string searchPattern)
        {
            if (string.IsNullOrEmpty(searchPattern))
            {
                throw new ArgumentNullException("searchPattern");
            }

            var directoryEnd = searchPattern.LastIndexOf('\\');
            var directoryPath = searchPattern.Substring(0, directoryEnd + 1);
            var filePath = searchPattern.Substring(directoryEnd + 1);

            var files = Directory.GetFiles(directoryPath, filePath);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Retries file action code block for maxRetries times If file locked.
        /// </summary>
        /// <param name="source"> FileInfo object </param>
        /// <param name="codeBlock"> The code block </param>
        /// <param name="retryIntervalInMilliseconds">An action to be executed in the case of an exception</param>
        /// <param name="maxRetries">The total time in milliseconds before no more retries should be attempted</param>
        public static void RetryFileActionIfLocked(this FileInfo source, Action<FileInfo> codeBlock, int retryIntervalInMilliseconds = 100, int maxRetries = 5)
        {
            if (codeBlock == null)
            {
                // no action just return as there’s nothing to do
                return;
            }
            var tries = 0;
            var filelocked = true;
            while (filelocked)
            {
                try
                {
                    codeBlock(source);
                    filelocked = false;
                }
                catch (IOException ioe) // catch these to work out if it’s a file locked error
                {
                    // is it a filelockederror?
                    if (IsFileLockedError(ioe))
                    {
                        // do we retry
                        if (++tries < maxRetries)
                        {
                            // go around again
                            Thread.Sleep(100);
                        }
                        else
                        {
                            // too many retries so log and exit loop
                            Logger.Error("Not succeeded after " + tries + " attempts. Returning false");
                            throw;
                        }
                    }
                    else
                    {
                        // different error so log and escalate to user don’t go around again regardless of number of tries
                        Logger.Error("IOException occured", ioe);
                        throw;
                    }
                }
            }
        }

        private static bool IsFileLockedError(Exception exception)
        {
            var code = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return code == 32 || code == 33;
        }
    }
}
