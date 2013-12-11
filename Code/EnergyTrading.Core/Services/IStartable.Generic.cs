namespace EnergyTrading.Services
{
    /// <summary>
    /// Interface to start and stop services
    /// </summary>
    /// <typeparam name="T">Type that is started.</typeparam>
    public interface IStartable<out T>
    {
        /// <summary>
        /// Starts the object, generally this MUST be called on a object that implements this interface
        /// </summary>
        /// <returns>An instance of the started object.</returns>
        T Start();

        /// <summary>
        /// Stops a object that can be started
        /// </summary>
        void Stop();
    }
}