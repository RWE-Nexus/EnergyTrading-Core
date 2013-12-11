using Microsoft.Practices.Unity;
using EnergyTrading.Extensions;
using EnergyTrading.FileProcessing.FileProcessors;

namespace EnergyTrading.FileProcessing.Registrars
{
    public class EventBasedProcessorDefaultRegistrar : IFileProcessorRegistrar
    {
        public void Register(IUnityContainer container, FileProcessorEndpoint endpoint)
        {
            // Registration for the handler if present
            if (endpoint.Handler != null)
            {
                container.RegisterType(typeof(IFileHandler), endpoint.Handler, endpoint.Name);
            }

            if (endpoint.PostProcessor == null)
            {
                if ((endpoint.Handler != null) && endpoint.Handler.Implements<IFilePostProcessor>())
                {
                    endpoint.PostProcessor = endpoint.Handler;
                }
                else
                {
                    endpoint.PostProcessor = typeof(NullPostProcessor);
                }
            }

            // Registration for the post processor
            container.RegisterType(typeof(IFilePostProcessor), endpoint.PostProcessor, endpoint.Name);
            container.RegisterType(typeof(IFileFilter), endpoint.AdditionalFilter, endpoint.Name);

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