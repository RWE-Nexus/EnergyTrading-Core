namespace EnergyTrading.ServiceModel.Loggers
{
    using System;
    using System.IO;

    using EnergyTrading.ServiceModel;

    public class FileMessageLogger : IMessageLogger
    {
        private static int messageId;
        private readonly object syncLock;

        public FileMessageLogger()
        {
            syncLock = new object();
        }

        public string Path { get; set; }

        public void Sent(string message)
        {
            WriteFile(message, ".sent.xml");
        }

        public void Received(string message)
        {
            WriteFile(message, ".recd.xml");
        }

        public void WriteFile(string message, string extension)
        {
            try
            {
                var fileName = Path;
                lock (syncLock)
                {
                    messageId++;
                    fileName += "message_" + messageId + extension;
                }

                using (var writer = new StreamWriter(fileName))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                // Just swallow on error
            }
        }
    }
}