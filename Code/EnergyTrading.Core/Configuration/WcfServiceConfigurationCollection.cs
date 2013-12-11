namespace EnergyTrading.Configuration
{
    public class WcfServiceConfigurationCollection : NamedConfigElementCollection<WcfServiceConfigurationElement>
    {
        protected override string ElementName
        {
            get { return "service"; }
        }
    }
}
