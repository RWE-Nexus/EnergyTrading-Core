namespace EnergyTrading.Services
{
    /// <summary>
    /// Interface to start and stop services
    /// </summary>
    public interface IStartable
    {
        /// <summary>
        /// Starts the object, generally this MUST be called on a object that implements this interface.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops a object that can be started.
        /// </summary>
        void Stop();
    }
}