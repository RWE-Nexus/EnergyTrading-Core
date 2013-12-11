namespace EnergyTrading.Container
{
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Marker interface for stuff that has a <see cref="IServiceLocator" />
    /// <para>
    /// Depending on how this was created/injected we might not have access so this
    /// interface exposes it for testing purposes.
    /// </para>
    /// </summary>
    public interface IServiceLocatorOwner
    {
        IServiceLocator Locator { get; }
    }
}