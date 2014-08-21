namespace EnergyTrading.Polling.Registrars
{
    using EnergyTrading.Polling;

    using Microsoft.Practices.Unity;

    public static class PollingContainerExtensions
    {
        public static void RegisterPollProcessor(this IUnityContainer container, PollProcessorEndpoint endpoint)
        {
            endpoint.Validate();

            container.RegisterInstance(endpoint.Name, endpoint);
            container.RegisterType(typeof(IPoller), endpoint.Handler, endpoint.Name);
            container.RegisterType(
                typeof(IPollProcessor),
                typeof(PollProcessor),
                endpoint.Name,
                new PerResolveLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<PollProcessorEndpoint>(endpoint.Name),
                    new ResolvedParameter<IPoller>(endpoint.Name)));
        }
    }
}
