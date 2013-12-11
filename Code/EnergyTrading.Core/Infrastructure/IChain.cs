namespace EnergyTrading.Infrastructure
{
    /// <summary>
    /// Abstraction of a Chain of Responsibility pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChain<T>
    {
        T Successor { get; set; }
    }
}