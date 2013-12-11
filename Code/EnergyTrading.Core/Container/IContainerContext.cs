namespace EnergyTrading.Container
{
    using System;

    /// <summary>
    /// Simple abstraction layer over containers so we can register without knowing the container
    /// </summary>
    public interface IContainerContext
    {
        void Register<TInterface, TImplementation>();

        void Register<TInterface, TImplementation>(string key);

        void Register<TInterface>(Type implementationType);

        void Register<TInterface>(Type implementationType, string key);

        void Register(Type serviceType, Type implementationType);

        void Register(Type serviceType, Type implementationType, string key);

        void RegisterInstance<TInterface>(TInterface instance);

        void RegisterInstance<TInterface>(TInterface instance, string key);

        void RegisterInstance(Type serviceType, object instance);

        void RegisterInstance(Type serviceType, object instance, string key);

        /// <summary>
        /// Release a component instance.
        /// </summary>
        /// <param name="instance"></param>
        void Release(object instance);
    }
}
