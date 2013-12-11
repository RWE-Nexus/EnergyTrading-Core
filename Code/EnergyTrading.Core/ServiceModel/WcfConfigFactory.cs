namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using EnergyTrading.Configuration;

    public class WcfConfigFactory
    {
        public IWcfConfig InitializeConfig()
        {
            return this.InitializeConfig("wcfServices");
        }

        public IWcfConfig InitializeConfig(string sectionName)
        {
            var wcfServices = (WcfServiceConfigurationSection)ConfigurationManager.GetSection(sectionName);
            var config = new WcfConfig(wcfServices.IsConsoleMode);

            foreach (WcfServiceConfigurationElement item in wcfServices.Services)
            {
                var hostType = Type.GetType(item.HostTypeDeclaration);
                var contractType = Type.GetType(item.ContractTypeDeclaration);
                var configItem = new WcfServiceConfig
                {
                    Key = item.Name,
                    HostType = hostType,
                    ContractType = contractType,
                    EndpointName = item.EndpointName,
                    AuthorizedGroups = new List<string>(item.AuthorizedGroups.Split(','))
                };

                config.Add(configItem);
            }

            return config;
        }
    }
}
