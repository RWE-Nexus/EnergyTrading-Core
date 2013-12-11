namespace EnergyTrading.Container.Unity
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Constructs a service host using an existing Unity container
    /// </summary>
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        public UnityServiceHostFactory() : this(string.Empty)
        {            
        }

        public UnityServiceHostFactory(string containerName) : this(ServiceLocator.Current.GetInstance<IUnityContainer>(containerName))
        {            
        }

        public UnityServiceHostFactory(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        public IUnityContainer Container { get; set; }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var serviceHost = new UnityServiceHost(serviceType, Container, baseAddresses);

            return serviceHost;
        }
    }
}