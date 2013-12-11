using EnergyTrading.FileProcessing.Configuration;
using EnergyTrading.Services;

namespace EnergyTrading.FileProcessing.Registrars
{
    using Microsoft.Practices.Unity;

    using EnergyTrading.FileProcessing;

    public static class ContainerExtensions
    {
        public static void RegisterProcessor(this IUnityContainer container, FileProcessorEndpoint endpoint)
        {
            endpoint.Validate();

            // Register the endpoint instance under the name, needed for the processor
            container.RegisterInstance(endpoint.Name, endpoint);

            // create file processor configurator instance and invoke
            var typeResolver = container.Resolve<ITypeResolver>();
            var fileProcessorRegistrar = typeResolver.Resolve<IFileProcessorRegistrar>(endpoint.ProcessorConfigurator);
            fileProcessorRegistrar.Register(container, endpoint);
        }
    }
}