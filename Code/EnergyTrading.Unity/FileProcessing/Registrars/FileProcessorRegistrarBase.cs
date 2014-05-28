namespace EnergyTrading.FileProcessing.Registrars
{
    using EnergyTrading.Container.Unity.AutoRegistration;

    using Microsoft.Practices.Unity;

    public abstract class FileProcessorRegistrarBase : IFileProcessorRegistrar
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

            this.RegisterProcessor(container, endpoint);
        }

        protected abstract void RegisterProcessor(IUnityContainer container, FileProcessorEndpoint endpoint);
    }
}