using System.ComponentModel;

namespace EnergyTrading.Polling.Configuration
{
    using System.Configuration;

    using EnergyTrading.Configuration;

    public class PollProcessorElement : NamedConfigElement
    {
        [ConfigurationProperty("intervalSecs", DefaultValue = 60)]
        public virtual int IntervalSecs
        {
            get { return (int)this["intervalSecs"]; }
            set { this["intervalSecs"] = value; }
        }

        [ConfigurationProperty("handler", IsRequired = true)]
        public virtual string Handler
        {
            get { return (string)this["handler"]; }
            set { this["handler"] = value; }
        }

        [ConfigurationProperty("singlePolling", DefaultValue = true)]
        public virtual bool SinglePolling
        {
            get { return (bool)this["singlePolling"]; }
            set { this["singlePolling"] = value; }
        }

        [ConfigurationProperty("workers", DefaultValue = 1)]
        public virtual int Workers
        {
            get { return (int)this["workers"]; }
            set { this["workers"] = value; }
        }
    }
}
