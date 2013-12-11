namespace EnergyTrading.ServiceModel
{
    using System;

    public class WcfServiceClientFactory : IWcfServiceClientFactory
    {
        private readonly IWcfConfig config;
        
        public WcfServiceClientFactory(IWcfConfig config)
        {
            this.config = config;
        }

        public WcfServiceClient<TClient> Create<TClient>(string key) where TClient : class
        {
            var clientType = typeof(TClient);
            var configItem = config.Get(clientType);
            if (configItem != null)
            {
                var tcpBinding = TcpBindingUtility.CreateNetTcpBinding();
                var endpointAddress = TcpBindingUtility.CreateEndpointAddress(configItem.EndpointAddress);

                var client = new WcfServiceClient<TClient>(tcpBinding, endpointAddress);
                return client;
            }

            throw new Exception("Type " + clientType.AssemblyQualifiedName + " not found.");
        }
    }
}
