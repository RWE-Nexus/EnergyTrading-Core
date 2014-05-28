using Microsoft.Practices.Unity;

namespace EnergyTrading.FileProcessing.Registrars
{
    using EnergyTrading.FileProcessing;
    using EnergyTrading.FileProcessing.FileProcessors;

    public class EventBasedProcessorDefaultRegistrar : FileProcessorRegistrarBase
    {
        protected override void RegisterProcessor(IUnityContainer container, FileProcessorEndpoint endpoint)
        {
            // Tie the parameters to the processor
            container.RegisterType(
                typeof(IFileProcessor),
                typeof(EventBasedFileProcessor),
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