namespace EnergyTrading.Container.Unity
{
    using System;

    using Microsoft.Practices.Unity;

    public class UnityContainerContext : IContainerContext
    {
        private readonly IUnityContainer container;

        public UnityContainerContext(IUnityContainer container)
        {
            this.container = container;
        }

        public void Register<TInterface, TImplementation>()
        {
            this.Register(typeof(TInterface), typeof(TImplementation));
        }

        public void Register<TInterface, TImplementation>(string key)
        {
            this.Register<TInterface>(typeof(TImplementation), key);
        }

        public void Register<TInterface>(Type implementationType)
        {
            this.Register(typeof(TInterface), implementationType);
        }

        public void Register<TInterface>(Type implementationType, string key)
        {
            this.Register(typeof(TInterface), implementationType, key);
        }

        public void Register(Type serviceType, Type implementationType)
        {
            this.container.RegisterType(serviceType, implementationType);
        }

        public void Register(Type serviceType, Type implementationType, string key)
        {
            this.Register(serviceType, implementationType, key);
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            this.RegisterInstance(typeof(TInterface), instance);
        }

        public void RegisterInstance<TInterface>(TInterface instance, string key)
        {
            this.RegisterInstance(typeof(TInterface), instance, key);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            this.container.RegisterInstance(serviceType, instance);
        }

        public void RegisterInstance(Type serviceType, object instance, string key)
        {
            this.container.RegisterInstance(serviceType, key, instance);
        }

        public void Release(object instance)
        {            
        }
    }
}
