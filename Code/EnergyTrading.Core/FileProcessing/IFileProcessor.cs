namespace EnergyTrading.FileProcessing
{
    using EnergyTrading.Services;

    public interface IFileProcessor : IStartable
    {
        string Name { get; }
    }
}