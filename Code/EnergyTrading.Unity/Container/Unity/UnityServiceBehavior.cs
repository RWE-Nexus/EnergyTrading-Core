namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// 
    /// </summary>
    public class UnityServiceBehavior : IServiceBehavior
    {
        public UnityServiceBehavior(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        public IUnityContainer Container { get; set; }

        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceDescription == null)
            {
                throw new ArgumentNullException("serviceDescription");
            }

            if (serviceHostBase == null)
            {
                throw new ArgumentNullException("serviceHostBase");
            }

            for (var dispatcherIndex = 0;
                 dispatcherIndex < serviceHostBase.ChannelDispatchers.Count;
                 dispatcherIndex++)
            {
                var dispatcher = serviceHostBase.ChannelDispatchers[dispatcherIndex];
                var channelDispatcher = (ChannelDispatcher)dispatcher;
                for (var endpointIndex = 0; endpointIndex < channelDispatcher.Endpoints.Count; endpointIndex++)
                {
                    var endpointDispatcher = channelDispatcher.Endpoints[endpointIndex];
                    endpointDispatcher.DispatchRuntime.InstanceProvider = 
                        new UnityInstanceProvider(this.Container, serviceDescription.ServiceType);
                }
            } 
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
