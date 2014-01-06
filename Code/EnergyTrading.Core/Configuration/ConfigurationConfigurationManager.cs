namespace EnergyTrading.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public class ConfigurationConfigurationManager : IConfigurationManager
    {
        private readonly Configuration configuration;
        private NameValueCollection appSettingsNvc;

        public ConfigurationConfigurationManager(Configuration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            this.configuration = config;
        }

        public object GetSection(string sectionName)
        {
            return this.configuration.GetSection(sectionName);
        }

        private static NameValueCollection BuildAppSettingsCollection(AppSettingsSection section)
        {
            var nvc = new NameValueCollection();
            foreach (KeyValueConfigurationElement pair in section.Settings)
            {
                nvc.Add(pair.Key, pair.Value);
            }
            return nvc;
        }

        public NameValueCollection AppSettings
        {
            get
            {
                return this.appSettingsNvc ?? (this.appSettingsNvc = BuildAppSettingsCollection(this.configuration.AppSettings));
            }
        }

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                return this.configuration.ConnectionStrings.ConnectionStrings;
            }
        }
    }
}
