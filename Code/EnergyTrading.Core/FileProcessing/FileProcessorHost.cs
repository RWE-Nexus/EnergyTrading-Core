namespace EnergyTrading.FileProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using EnergyTrading.Logging;

    /// <summary>
    /// Hosts the collection of <see cref="IFileProcessor" />s.
    /// </summary>
    public class FileProcessorHost : IFileProcessorHost
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<IFileProcessor> processors;
        
        /// <summary>
        /// Construct a new FileProcessorHost
        /// </summary>
        /// <param name="processors"></param>
        /// <remarks>Have to use array rather then IList due to Unity restrictions</remarks>
        public FileProcessorHost(IFileProcessor[] processors)
        {
            if (processors == null)
            {
                throw new ArgumentNullException("processors");
            }

            this.processors = new List<IFileProcessor>(processors);      
        }

        /// <summary>
        /// Tells all configured processors to start.
        /// </summary>
        public void Start()
        {
            // Clean down existing state.
            this.Stop();

            Logger.Info("Starting");

            try
            {
                // And start each of the processors
                foreach (var processor in this.processors)
                {
                    processor.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error occured starting Processors.", ex);
                this.Stop();
                throw;
            }

            Logger.Info("Started");
        }

        /// <summary>
        /// Tell all configured processors to stop.
        /// </summary>
        public void Stop()
        {
            Logger.Info("Stopping");

            // Synchronous - problem?
            try
            {
                foreach (var processor in this.processors)
                {
                    processor.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error occured stopping Processors.", ex);
                throw;
            }

            Logger.Info("Stopped");
        }
    }
}