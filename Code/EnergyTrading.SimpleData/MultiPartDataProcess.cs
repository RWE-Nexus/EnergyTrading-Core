namespace EnergyTrading.Data.SimpleData
{
    using System;
    using System.Reflection;

    using EnergyTrading.Logging;

    public class MultiPartDataProcess
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int partSize;

        private readonly Func<string, int, int, bool> partProcessor;
        private readonly Func<string, bool> startProcessing;
        private readonly Func<string, bool> endProcessing;

        public MultiPartDataProcess(Func<string, int, int, bool> partProcessor, Func<string, bool> startProcessing = null, Func<string, bool> endProcessing = null, int partSize = 30000)
        {
            if (partProcessor == null)
            {
                throw new ArgumentNullException("partProcessor");
            }

            if (partSize <= 0)
            {
                throw new ArgumentOutOfRangeException("partSize", "Value must be greater than 0");
            }

            this.partProcessor = partProcessor;
            this.startProcessing = startProcessing;
            this.endProcessing = endProcessing;
            this.partSize = partSize;
        }

        public bool ProcessData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Logger.Debug("MultiPartDataProcess no work as data is null or empty");
                return true;
            }

            if ((this.startProcessing != null) && !this.startProcessing(data))
            {
                Logger.Debug("Failed to start processing for Multi part data");
                return false;
            }

            var totalChunks = (data.Length / this.partSize) + 1;
            var currentChunk  = 1;
            var numberWritten = 0;
            var processedPart = true;
            while((numberWritten < data.Length) && processedPart)
            {
                var dataPart = (data.Length - numberWritten) < this.partSize ? 
                                        data.Substring(numberWritten) : 
                                        data.Substring(numberWritten, this.partSize);
                processedPart = this.partProcessor(dataPart, currentChunk++, totalChunks);
                if (processedPart)
                {
                    numberWritten += dataPart.Length;
                }
                else
                {
                    Logger.Debug(string.Format("Failed to process part {0} of {1}", currentChunk - 1, totalChunks));
                    return false;
                }
            }

            if (numberWritten == data.Length)
            {
                if ((this.endProcessing != null) && !this.endProcessing(data))
                {
                    Logger.Debug("End processing of Multi part data failed");
                    return false;
                }
                return true;
            }
            Logger.Debug(string.Format("Incorrect amount of data written {0} out of {1}", numberWritten, data.Length));
            return false;
        }
    }
}