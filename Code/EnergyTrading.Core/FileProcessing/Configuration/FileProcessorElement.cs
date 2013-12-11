using System;

namespace EnergyTrading.FileProcessing.Configuration
{
    using System.Configuration;

    using EnergyTrading.Configuration;

    /// <summary>
    /// Configuration definition for a file processor.
    /// </summary>
    public class FileProcessorElement : NamedConfigElement
    {
        /// <summary>
        /// Gets or sets the file drop path to monitor.
        /// </summary>
        [ConfigurationProperty("dropPath", IsRequired = true)]
        public virtual string DropPath
        {
            get { return (string)this["dropPath"]; }
            set { this["dropPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the path where in-progress files are written to (used by the polling based processor).
        /// </summary>
        [ConfigurationProperty("inProgressPath")]
        public virtual string InProgressPath
        {
            get { return (string)this["inProgressPath"]; }
            set { this["inProgressPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the file drop path to monitor.
        /// </summary>
        [ConfigurationProperty("filter")]
        public virtual string Filter
        {
            get { return (string)this["filter"]; }
            set { this["filter"] = value; }
        }

        /// <summary>
        /// Gets or sets whether we should monitor subdirecties of the drop path.
        /// </summary>
        [ConfigurationProperty("monitorSubdirectories")]
        public virtual bool MonitorSubdirectories
        {
            get { return (bool)this["monitorSubdirectories"]; }
            set { this["monitorSubdirectories"] = value; }
        }

        /// <summary>
        /// Gets or sets the success path to move files into
        /// </summary>
        [ConfigurationProperty("successPath")]
        public virtual string SuccessPath
        {
            get { return (string)this["successPath"]; }
            set { this["successPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the success path to move files into
        /// </summary>
        [ConfigurationProperty("failurePath", IsRequired = true)]
        public virtual string FailurePath
        {
            get { return (string)this["failurePath"]; }
            set { this["failurePath"] = value; }
        }

        /// <summary>
        /// Gets or sets the type name of the processor configurator type to use.  Leaving it blank will default to
        /// the event based processor being used.
        /// </summary>
        [ConfigurationProperty("processorConfiguratorType", DefaultValue = "EventBased")]
        public virtual string ProcessorConfiguratorType
        {
            get { return (string)this["processorConfiguratorType"]; }
            set { this["processorConfiguratorType"] = value; }
        }

        /// <summary>
        /// Gets or sets the type name of the handler to use.
        /// </summary>
        /// <remarks>
        /// This item is optional but if not present the user is responsible
        /// for registering an appropriate <see cref="IFileHandler" /> into
        /// the container against the listener's name.
        /// </remarks>
        [ConfigurationProperty("handler")]
        public virtual string Handler
        {
            get { return (string)this["handler"]; }
            set { this["handler"] = value; }
        }

        /// <summary>
        /// Gets or sets the scavenge process interval.
        /// </summary>
        [ConfigurationProperty("scavengeInterval", DefaultValue = 30)]
        public virtual int ScavengeInterval
        {
            get { return (int)this["scavengeInterval"]; }
            set { this["scavengeInterval"] = value; }
        }

        /// <summary>
        /// Gets or sets the elapsed processing time before we recover a file.
        /// </summary>
        [ConfigurationProperty("recoveryInterval", DefaultValue = 60)]
        public virtual int RecoveryInterval
        {
            get { return (int)this["recoveryInterval"]; }
            set { this["recoveryInterval"] = value; }
        }

        /// <summary>
        /// Gets or sets the time in seconds of inactivity before we alert the polling processor
        /// to restart itself.  If explicitly set to 0 the restart mechanism will not be used.
        /// </summary>
        [ConfigurationProperty("pollingInactivityRestartInterval", DefaultValue = 60)]
        public virtual int PollingInactivityRestartInterval
        {
            get { return (int)this["pollingInactivityRestartInterval"]; }
            set { this["pollingInactivityRestartInterval"] = value; }
        }

        /// <summary>
        /// Gets or sets the number of consumers spawned in the polling based processor.
        /// </summary>
        [ConfigurationProperty("consumers", DefaultValue = 1)]
        public virtual int Consumers
        {
            get { return (int)this["consumers"]; }
            set { this["consumers"] = value; }
        }

        /// <summary>
        /// Gets or sets the type name of the post processor <see cref="IFilePostProcessor" /> to use
        /// </summary>
        /// <remarks>
        /// This item is optional. 
        /// If not present the handler type will be checked for post processing capability
        /// before it is assumed that there are no post processing requirements
        /// </remarks>
        [ConfigurationProperty("postProcessor")]
        public virtual string PostProcessor 
        {
            get { return (string)this["postProcessor"]; }
            set { this["postProcessor"] = value; }
        }


        [ConfigurationProperty("additionalFilter")]
        public virtual string AdditionalFilter
        {
            get
            {
                return (string)this["additionalFilter"];
            }
            set
            {
                this["additionalFilter"] = value;
            }
        }
    }
}