namespace EnergyTrading.Configuration
{
    using System.Configuration;

    /*
        <wcfServices consoleMode="On">
            <add key="test1" 
                serviceAddressPort="localhost:2981" 
                endpointName="Test1EndPoint" 
                authorizedGroups="WcfServiceClients" 
                hostType="Test1Service.MyService, Test1Service"
                contractType="Test1Common.IMyService, Test1Common" />
            <add key="test2" 
                serviceAddressPort="localhost:2981" 
                endpointName="Test2EndPoint" 
                authorizedGroups="WcfServiceClients" 
                hostType="Test2Service.MyService, Test2Service"
                contractType="Test2Common.IMyService, Test2Common" />
        </wcfServices>
    */
    public sealed class WcfServiceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("services", IsDefaultCollection = true)]
        public WcfServiceConfigurationCollection Services
        {
            get
            {
                return (WcfServiceConfigurationCollection) base["services"];
            }
        }

        //<wcfServices consoleMode="On">
        [ConfigurationProperty("consoleMode", DefaultValue = "On", IsKey = false, IsRequired = true)]
        [RegexStringValidator("^(On|on|Off|off)$")]
        public string ConsoleMode
        {
            get
            {
                return (string)this["consoleMode"];
            }
            set
            {
                this["consoleMode"] = value;
            }
        }

        public bool IsConsoleMode
        {
            get
            {
                return this.ConsoleMode.ToUpperInvariant() == "ON";
            }
        }
    }
}