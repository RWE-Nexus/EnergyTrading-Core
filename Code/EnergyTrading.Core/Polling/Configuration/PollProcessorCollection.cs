namespace EnergyTrading.Polling.Configuration
{
    using EnergyTrading.Configuration;

    public class PollProcessorCollection : NamedConfigElementCollection<PollProcessorElement>
    {
        protected override string ElementName
        {
            get { return "pollProcessor"; }
        }
    }
}
