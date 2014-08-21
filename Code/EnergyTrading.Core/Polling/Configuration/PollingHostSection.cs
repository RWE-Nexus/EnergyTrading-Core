namespace EnergyTrading.Polling.Configuration
{
    using System.Configuration;

    public class PollingHostSection : ConfigurationSection
    {
        [ConfigurationProperty("pollProcessors")]
        public PollProcessorCollection PollProcessors
        {
            get
            {
                return (PollProcessorCollection)this["pollProcessors"];
            }
            set
            {
                this["pollProcessors"] = value;
            }
        }
    }
}
