namespace EnergyTrading.Polling.Configuration
{
    using System;
    using System.Configuration;
    using System.Linq;

    using EnergyTrading.Configuration;
    using EnergyTrading.Container.Unity;
    using EnergyTrading.Polling.Registrars;
    using EnergyTrading.Services;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    public class PollingHostConfigurator
    {
        public PollingHostConfigurator() : this(new UnityContainer())
        {
        }

        public PollingHostConfigurator(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        public IUnityContainer Container { get; private set; }

        private bool ExecuteConfigTasks { get; set; }

        public PollingHostConfigurator ConfigureContainer()
        {
            this.Container.InstallCoreExtensions();

            // Self-register and set up service location 
            var locator = new UnityServiceLocator(this.Container);

            // Load information from section if present.
            var section = ConfigurationManager.GetSection("unity");
            if (section != null)
            {
                this.Container.LoadConfiguration();
            }

            return this;
        }

        public PollingHostConfigurator ConfigureRegistrars()
        {
            this.Container.ConfigureRegistrars();

            return this;
        }

        public virtual PollingHostConfigurator StandardRegistrars()
        {
            this.Register<PollingHostRegistrar>();

            return this;
        }

        public PollingHostConfigurator Configure<T>()
            where T : IContainerRegistrar, new()
        {
            var registrar = new T();
            registrar.Register(this.Container);

            return this;
        }

        public IPollingHost ToPollingHost()
        {
            return this.Execute<IPollingHost>();
        }

        public PollingHostConfigurator Register<T>()
            where T : IContainerRegistrar
        {
            this.Container.RegisterType<IContainerRegistrar, T>(typeof(T).FullName);

            return this;
        }

        public PollingHostConfigurator RunConfigurationTasks()
        {
            this.ExecuteConfigTasks = true;

            return this;
        }

        public void Shutdown()
        {
            this.Container.ResolveAll<IShutdownTask>()
                .ToList()
                .ForEach(t => t.Execute());
        }

        protected T Execute<T>()
            where T : IStartable
        {
            if (this.ExecuteConfigTasks)
            {
                ConfigurationBootStrapper.Initialize(this.Container.ResolveAll<IGlobalConfigurationTask>());
            }

            var host = this.Container.Resolve<T>();
            host.Start();

            return host;
        }
    }
}
