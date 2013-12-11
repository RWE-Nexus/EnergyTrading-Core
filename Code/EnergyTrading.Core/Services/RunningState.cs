namespace EnergyTrading.Services
{
    /// <summary>
    /// State model for a service.
    /// </summary>
    public enum RunningState
    {
        /// <summary>
        /// Service is not running.
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// Transitioning from <see cref="Started" /> to <see cref="Stopped" />
        /// </summary>
        Stopping = 1,

        /// <summary>
        /// Transitioning from <see cref="Stopped" /> to <see cref="Started" />
        /// </summary>
        Starting = 2,

        /// <summary>
        /// Service is running.
        /// </summary>
        Started = 3
    }
}