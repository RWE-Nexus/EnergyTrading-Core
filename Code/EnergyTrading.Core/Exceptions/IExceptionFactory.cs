namespace EnergyTrading.Exceptions
{
    using System;

    /// <summary>
    /// Creates an exception from another
    /// </summary>
    public interface IExceptionFactory
    {
        /// <summary>
        /// Converts an exception into a different one
        /// </summary>
        /// <param name="exception">Exception to convert</param>
        /// <returns>new exception or null if we can't convert</returns>
        Exception Convert(Exception exception);
    }
}