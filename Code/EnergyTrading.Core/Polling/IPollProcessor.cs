namespace EnergyTrading.Polling
{
    using EnergyTrading.Services;

    public interface IPollProcessor : IStartable
    {
        string Name { get; }
    }
}
