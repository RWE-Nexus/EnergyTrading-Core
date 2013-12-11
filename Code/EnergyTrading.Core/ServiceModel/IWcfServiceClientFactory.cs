namespace EnergyTrading.ServiceModel
{
    public interface IWcfServiceClientFactory
    {
        WcfServiceClient<TClient> Create<TClient>(string key) 
            where TClient : class;
    }
}