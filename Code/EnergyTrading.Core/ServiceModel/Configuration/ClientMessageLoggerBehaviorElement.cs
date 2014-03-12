namespace EnergyTrading.ServiceModel.Configuration
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Configuration;

    using EnergyTrading.ServiceModel.Channels;

    /// <summary>
    /// Enables the use of <see cref="ClientMessageLoggerBehavior" /> from a machine or application configuration files.
    /// </summary>
    public class ClientMessageLoggerBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Gets or sets the logger type.
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.Logger, DefaultValue = ConfigurationStrings.DefaultLogger)]
        public string Logger
        {
            get { return (string)base[ConfigurationStrings.Logger]; }
            set { base[ConfigurationStrings.Logger] = value; }
        }

        /// <summary>
        /// Gets or sets the logging path.
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.Path)]
        public string Path
        {
            get { return (string)base[ConfigurationStrings.Path]; }
            set { base[ConfigurationStrings.Path] = value; }
        }

        /// <inheritdoc />
        public override Type BehaviorType
        {
            get { return typeof(ClientMessageLoggerBehavior); }
        }

        /// <inheritdoc />
        protected override object CreateBehavior()
        {
            return new ClientMessageLoggerBehavior
            {
                Logger = MessageLoggerFactory.Create(Logger, Path)
            };
        }
    }
}