namespace EnergyTrading.Configuration
{
    /// <summary>
    /// A task that should be performed at application shutdown
    /// </summary>
    public interface IShutdownTask
    {
        void Execute();
    }
}