namespace EnergyTrading.Container.Unity
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Simple marker extension to know that we have initialized a container.
    /// </summary>
    /// <remarks>
    /// Needed so that we don't invoke <see cref="UnityExtensions.InstallCoreExtensions" /> more than once
    /// which causes issues with resolving IServiceLocator.
    /// </remarks>
    public class InitialisedExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
        }
    }
}
