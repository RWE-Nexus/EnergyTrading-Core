namespace EnergyTrading.Logging
{
    using System;

    /// <summary>
    /// Static logger factory that provides access to loggers.
    /// <para>
    /// Initialization is similar to ServiceLocator 
    /// </para>
    /// </summary>
    public static class LoggerFactory
    {
        private static Func<ILoggerFactory> provider = () => new SimpleLoggerFactory(new NullLogger());

        public static void SetProvider(Func<ILoggerFactory> factory)
        {
            provider = factory;
        }

        public static void Initialize()
        {
            provider().Initialize();
        }

        /// <summary>
        /// Get a named logger
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILogger GetLogger(string name)
        {
            return provider().GetLogger(name);
        }

        /// <summary>
        /// Get a logger for a type
        /// </summary>
        /// <returns></returns>
        public static ILogger GetLogger<T>()
        {
            return provider().GetLogger<T>();
        }

        /// <summary>
        /// Get a logger for a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILogger GetLogger(Type type)
        {
            return provider().GetLogger(type);
        }
    }
}