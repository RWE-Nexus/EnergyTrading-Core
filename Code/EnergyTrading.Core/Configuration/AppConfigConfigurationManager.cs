namespace EnergyTrading.Configuration
{
    using System.Configuration;

    public class AppConfigConfigurationManager : IConfigurationManager
    {
        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }

        public System.Collections.Specialized.NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        public object GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }
    }
}