namespace EnergyTrading.Services
{
    /// <summary>
    /// Interface to determine state of a service.
    /// </summary>
    public interface IRunningState
    {
        /// <summary>
        /// Get the current state of the service.
        /// </summary>
        RunningState RunningState { get; }
    }
}