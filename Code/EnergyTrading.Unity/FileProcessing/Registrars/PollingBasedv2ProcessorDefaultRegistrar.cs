namespace EnergyTrading.FileProcessing.Registrars
{
    using EnergyTrading.FileProcessing.FileProcessors;

    using Microsoft.Practices.Unity;

    public class PollingBasedv2ProcessorDefaultRegistrar : FileProcessorRegistrarBase
    {
        protected override void RegisterProcessor(IUnityContainer container, FileProcessorEndpoint endpoint)
        {
            // Tie the parameters to the processor
            container.RegisterType(
                typeof(IFileProcessor),
                typeof(PollingBasedv2FileProcessor),
                endpoint.Name,
                new PerResolveLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<FileProcessorEndpoint>(endpoint.Name),
                    new ResolvedParameter<IFileHandler>(endpoint.Name),
                    new ResolvedParameter<IFilePostProcessor>(endpoint.Name),
                    new ResolvedParameter<IFileFilter>(endpoint.Name)));
        }
    }
}