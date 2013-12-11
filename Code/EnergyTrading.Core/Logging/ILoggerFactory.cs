namespace EnergyTrading.Logging
{
    using System;

    /// <summary>
    /// Delivers loggers for types.
    /// <para>
    /// Determining an appropriate logger can be expensive and should be performed infrequently,
    /// typically by assigning a static variable in a class.
    /// </para>
    /// </summary>
    /// <example>
    /// <c>
    /// public class Sample
    /// {
    ///     private static readonly ILogger loggerlogger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    /// }
    /// </c>
    /// </example>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Determine a logger for a name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ILogger GetLogger(string name);

        /// <summary>
        /// Determine a logger for a type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ILogger GetLogger<T>();

        /// <summary>
        /// Determine a logger for a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ILogger GetLogger(Type type);

        /// <summary>
        /// Initialize the logging system.
        /// </summary>
        void Initialize();             
        
        /// <summary>
        /// Shuts down the logging system safely.
        /// </summary>
        void Shutdown();
    }
}