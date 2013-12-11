namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Unity WCF service instance provider.
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
            this.ServiceType = serviceType;
        }

        public IUnityContainer Container { get; set; }

        public Type ServiceType { get; set; }

        /// <summary>
        /// Get Service instace via unity container 
        /// </summary>
        /// <param name="instanceContext"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var instance = this.Container.Resolve(this.ServiceType);

            if (instance == null)
            {
                const string MessageFormat = "No unity configuration was found for service type '{0}'";
                var failureMessage = string.Format(CultureInfo.InvariantCulture, MessageFormat, this.ServiceType.FullName);
                throw new ConfigurationErrorsException(failureMessage);
            }

            return instance;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            Container.Teardown(instance);
        }
    }
}