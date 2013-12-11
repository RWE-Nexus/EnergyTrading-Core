namespace EnergyTrading.FileProcessing
{
    using System;

    /// <summary>
    /// Endpoint definition for a file processor.
    /// </summary>
    public class FileProcessorEndpoint
    {
        public const string PollingBasedConfiguratorType = "EnergyTrading.FileProcessing.Registrars.PollingBasedProcessorDefaultRegistrar, EnergyTrading.Unity";
        public const string EventBasedConfiguratorType = "EnergyTrading.FileProcessing.Registrars.EventBasedProcessorDefaultRegistrar, EnergyTrading.Unity";

        public string Name { get; set; }

        public string DropPath { get; set; }

        public string InProgressPath { get; set; }

        public string Filter { get; set; }

        public bool MonitorSubdirectories { get; set; }

        public string SuccessPath { get; set; }

        public string FailurePath { get; set; }

        /// <summary>
        /// Gets or sets the type of the processor configurator.
        /// </summary>
        public string ProcessorConfigurator { get; set; }

        /// <summary>
        /// Gets or sets the type of the handler.
        /// </summary>
        public Type Handler { get; set; }

        /// <summary>
        /// Gets or sets the type of the post processor
        /// </summary>
        public Type PostProcessor { get; set; }

        /// <summary>
        /// Frequency that we check for files that have hung during processing.
        /// </summary>
        public TimeSpan ScavengeInterval { get; set; }

        /// <summary>
        /// Amount of time that a file can be processed before it is re-added to the queue.
        /// </summary>
        public TimeSpan RecoveryInterval { get; set; }

        /// <summary>
        /// The number of consumer threads spawned within the loop based file processor mechanism.
        /// </summary>
        public int NumberOfConsumers { get; set; }

        /// <summary>
        /// The inactivity interval used by the polling processor to determine if it should restart the processor
        /// </summary>
        public TimeSpan PollingRestartInterval { get; set; }

        public Type AdditionalFilter { get; set; }

        /// <summary>
        /// Check we are valid to create an endpoint
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new NotSupportedException("Must supply a name");
            }

            if (string.IsNullOrWhiteSpace(ProcessorConfigurator))
            {
                throw new NotSupportedException("Must supply a process configurator");
            }

            if (ProcessorConfigurator == PollingBasedConfiguratorType)
            {
                if (string.IsNullOrWhiteSpace(InProgressPath))
                {
                    throw new NotSupportedException("Must supply the in-progress path");
                }
            }

            return true;
        }
    }
}