using Microsoft.Practices.Unity;
using EnergyTrading.Extensions;
using EnergyTrading.FileProcessing.FileHandling;
using EnergyTrading.FileProcessing.FileProcessors;

namespace EnergyTrading.FileProcessing.Registrars
{
    public class PollingBasedProcessorDefaultRegistrar : IFileProcessorRegistrar
    {
        public void Register(IUnityContainer container, FileProcessorEndpoint fileProcessorEndpoint)
        {
            // Registration for the handler if present
            if (fileProcessorEndpoint.Handler != null)
            {
                container.RegisterType(typeof(IFileHandler), fileProcessorEndpoint.Handler, fileProcessorEndpoint.Name);
            }

            container.RegisterType<IFileProcessResultHandler, StrategyFileEventHandler>(
                fileProcessorEndpoint.Name,
                new InjectionConstructor(new DeleteSuccessfulFileHandlingStrategy(),
                                         new MoveFileHandlingStrategy(fileProcessorEndpoint.DropPath),
                                         new MoveFileHandlingStrategy(fileProcessorEndpoint.FailurePath)));

            if (fileProcessorEndpoint.PostProcessor == null)
            {
                if ((fileProcessorEndpoint.Handler != null) && fileProcessorEndpoint.Handler.Implements<IFilePostProcessor>())
                {
                    fileProcessorEndpoint.PostProcessor = fileProcessorEndpoint.Handler;
                }
                else
                {
                    fileProcessorEndpoint.PostProcessor = typeof(NullPostProcessor);
                }
            }

            // Registration for the post processor
            container.RegisterType(typeof(IFilePostProcessor), fileProcessorEndpoint.PostProcessor, fileProcessorEndpoint.Name);
            container.RegisterType(typeof(IFileFilter), fileProcessorEndpoint.AdditionalFilter, fileProcessorEndpoint.Name);

            if (fileProcessorEndpoint.PollingRestartInterval.TotalSeconds > 0)
            {
                RegisterWithRestartMechanism(container, fileProcessorEndpoint);
            }
            else
            {
                RegisterWithoutRestartMechanism(container, fileProcessorEndpoint);
            }
        }

        public void RegisterWithRestartMechanism(IUnityContainer container, FileProcessorEndpoint fileProcessorEndpoint)
        {
            var producerConsumerQueue = new FileProducerConsumerQueue(fileProcessorEndpoint.NumberOfConsumers,
                                                                      container.Resolve<IFileProcessResultHandler>(fileProcessorEndpoint.Name),
                                                                      container.Resolve<IFileHandler>(fileProcessorEndpoint.Name));

            var fileThroughputAlerter = new FileThroughPutAlerter(producerConsumerQueue, fileProcessorEndpoint.PollingRestartInterval);

            var fileProcessor = new PollingBasedFileProcessor(fileProcessorEndpoint,
                                                              fileThroughputAlerter, 
                                                              container.Resolve<IFileFilter>(fileProcessorEndpoint.Name));

            fileThroughputAlerter.Alert += (sender, args) => fileProcessor.Restart();

            container.RegisterInstance(typeof(IHandleFiles),
                                   fileProcessorEndpoint.Name,
                                   fileThroughputAlerter);

            container.RegisterInstance(typeof (IFileProcessor),
                                   fileProcessorEndpoint.Name,
                                   fileProcessor);
        }

        public void RegisterWithoutRestartMechanism(IUnityContainer container, FileProcessorEndpoint fileProcessorEndpoint)
        {
            container.RegisterType(typeof(IHandleFiles),
                                   typeof(FileProducerConsumerQueue),
                                   fileProcessorEndpoint.Name,
                                   new PerResolveLifetimeManager(),
                                   new InjectionConstructor(
                                       fileProcessorEndpoint.NumberOfConsumers,
                                       new ResolvedParameter<IFileProcessResultHandler>(fileProcessorEndpoint.Name),
                                       new ResolvedParameter<IFileHandler>(fileProcessorEndpoint.Name)));

            container.RegisterType(typeof (IFileProcessor),
                                   typeof (PollingBasedFileProcessor),
                                   fileProcessorEndpoint.Name,
                                   new PerResolveLifetimeManager(),
                                   new InjectionConstructor(
                                       fileProcessorEndpoint,
                                       new ResolvedParameter<IHandleFiles>(fileProcessorEndpoint.Name)));
        }
    }
}