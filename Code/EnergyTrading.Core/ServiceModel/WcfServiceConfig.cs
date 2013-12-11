namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Collections.Generic;

    public class WcfServiceConfig
    {
        public string Key { get; set; }

        public Type HostType { get; set; }

        public Type ContractType { get; set; }

        public string ServiceAddressPort { get; set; }

        public string EndpointName{ get; set; }

        public string EndpointAddress
        {
            get { return ServiceAddressPort + "/" + EndpointName; }
        }

        public IList<string> AuthorizedGroups { get; set; }
    }
}