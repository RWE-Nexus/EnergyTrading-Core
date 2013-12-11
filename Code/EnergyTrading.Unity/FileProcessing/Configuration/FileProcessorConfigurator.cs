namespace EnergyTrading.FileProcessing.Configuration
{
    using System;
    using System.Configuration;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using EnergyTrading.Configuration;
    using EnergyTrading.Container.Unity;
    using EnergyTrading.FileProcessing.Registrars;
    using EnergyTrading.Mapping;
    using EnergyTrading.Services;

    /// <summary>
    /// Configures the file processor host.
    /// </summary>
    public class FileProcessorConfigurator
    {
        public FileProcessorConfigurator() : this(new UnityContainer())
        {
        }

        public FileProcessorConfigurator(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        /// <summary>
        /// Gets the container used by the configurator.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Gets or sets whether we should execute the configuration tasks.
        /// </summary>
        private bool ExecuteConfigTasks { get; set; }

        /// <summary>
        /// Default configuration of the container, including registration of service locator
        /// and loading of the container from the configuration file.
        /// </summary>
        /// <returns></returns>
        public FileProcessorConfigurator ConfigureContainer()
        {
            Container.InstallCoreExtensions();

            // Self-register and set up service location 
            var locator = new EnergyTradingUnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => locator);

            // Load information from section if present.
            var section = ConfigurationManager.GetSection("unity");
            if (section != null)
            {
                Container.LoadConfiguration();
            }

            return this;
        }

        /// <summary>
        /// Invokes all registrars registered with the container.
        /// </summary>
        /// <returns></returns>
        public FileProcessorConfigurator ConfigureRegistrars()
        {
            Container.ConfigureRegistrars();

            return this;
        }

        /// <summary>
        /// Register the standard registrations.
        /// </summary>
        /// <remarks>
        /// You must separately register a <see cref="IXmlMappingEngine" /> to
        /// support the message and event header serialization.
        /// </remarks>
        /// <returns></returns>
        public virtual FileProcessorConfigurator StandardRegistrars()
        {
            Register<FileProcessorHostRegistrar>();

            return this;
        }

        /// <summary>
        /// Creates and executes a container registrar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FileProcessorConfigurator Configure<T>()
            where T : IContainerRegistrar, new()
        {
            var registrar = new T();
            registrar.Register(Container);

            return this;
        }

        /// <summary>
        /// Resolve the publisher host and start the publisher.
        /// </summary>
        /// <returns></returns>
        public IFileProcessorHost ToProcessorHost()
        {
            return Execute<IFileProcessorHost>();
        }

        /// <summary>
        /// Registers a container registrar for later invocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FileProcessorConfigurator Register<T>() 
            where T : IContainerRegistrar
        {
            Container.RegisterType<IContainerRegistrar, T>(typeof(T).FullName);

            return this;
        }

        /// <summary>
        /// Set up to invoke the global configuration tasks.
        /// </summary>
        public FileProcessorConfigurator RunConfigurationTasks()
        {
            ExecuteConfigTasks = true;

            return this;
        }

        /// <summary>
        /// Invoke the <see cref="IShutdownTask" />s that are registered with the container.
        /// </summary>
        public void Shutdown()
        {
            Container.ResolveAll<IShutdownTask>()
                .ToList()
                .ForEach(t => t.Execute());            
        }

        protected T Execute<T>()
            where T : IStartable
        {
            if (ExecuteConfigTasks)
            {
                ConfigurationBootStrapper.Initialize(Container.ResolveAll<IGlobalConfigurationTask>());
            }

            var host = Container.Resolve<T>();
            host.Start();

            return host; 
        }   
    }
}