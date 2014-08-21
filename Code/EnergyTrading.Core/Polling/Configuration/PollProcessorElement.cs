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
    }
}
