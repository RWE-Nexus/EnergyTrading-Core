namespace EnergyTrading.Exceptions
{
    using System;

    /// <summary>
    /// Used to swallow exceptions
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Gets the Rethrow property.
        /// <para>
        /// Whether we should rethrow the exception irrespective of whether we've handled it
        /// </para>
        /// </summary>
        bool Rethrow { get; }

        /// <summary>
        /// Attempt to handle the exception
        /// </summary>
        /// <param name="ex">Exception to handle</param>
        /// <returns>true if we handled it, false otherwise</returns>
        bool Handle(Exception ex);
    }
}
