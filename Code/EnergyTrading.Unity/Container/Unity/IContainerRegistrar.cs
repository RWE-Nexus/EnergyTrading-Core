namespace EnergyTrading.Container.Unity
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Registration interfaces and their associated implementations into a Unity container.
    /// </summary>
    public interface IContainerRegistrar
    {
        /// <summary>
        /// Registers types/instances against a container 
        /// </summary>
        /// <param name="container">Container to use</param>
        void Register(IUnityContainer container);
    }
}