namespace EnergyTrading.Polling
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using EnergyTrading.Logging;

    public class PollingHost : IPollingHost
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<IPollProcessor> processors;

        public PollingHost(IPollProcessor[] pollProcessors)
        {
            if (pollProcessors == null)
            {
                throw new ArgumentNullException("pollProcessors");
            }

            this.processors = new List<IPollProcessor>(pollProcessors);
        }

        public void Start()
        {
            this.Stop();
            Logger.Info("Starting");

            try
            {
                foreach (var processor in this.processors)
                {
                    processor.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error occured starting poll processors.", ex);
                this.Stop();
                throw;
            }

            Logger.Info("Started");
        }

        public void Stop()
        {
            Logger.Info("Stopping");

            try
            {
                foreach (var processor in this.processors)
                {
                    processor.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error occured stopping poll processors.", ex);
                throw;
            }

            Logger.Info("Stopped");
        }
    }
}
