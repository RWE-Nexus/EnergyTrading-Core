namespace EnergyTrading.Logging.Log4Net
{
    using System;

    using EnergyTrading.Configuration;
    using EnergyTrading.Logging;

    using log4net;

    /// <summary>
    /// Logger factory for log4net.
    /// </summary>
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private readonly IConfigurationTask configurer;

        public Log4NetLoggerFactory() : this(null)
        {            
        }

        public Log4NetLoggerFactory(IConfigurationTask configurer)
        {
            this.configurer = configurer;
        }

        /// <summary>
        /// Returns a logger for the specified category.
        /// </summary>
        /// <param name="category">The category the logger is for.</param>
        /// <returns>A log4net ILogger implementation.</returns>
        public ILogger GetLogger(string category)
        {
            return Wrap(LogManager.GetLogger(category));
        }

        /// <summary>
        /// Returns a logger for the specified type.
        /// </summary>
        /// <typeparam name="T">The type the logger is for.</typeparam>
        /// <returns>A log4net ILogger implementation.</returns>
        public ILogger GetLogger<T>()
        {
            return this.GetLogger(typeof(T));
        }

        /// <summary>
        /// Returns a logger for the specified type.
        /// </summary>
        /// <param name="type">The type the logger is for.</param>
        /// <returns>A log4net ILogger implementation.</returns>
        public ILogger GetLogger(Type type)
        {
            return Wrap(LogManager.GetLogger(type));
        }

        /// <summary>
        /// Configures log4net, if a configurator was supplied.
        /// </summary>
        public void Initialize()
        {
            if (this.configurer == null)
            {
                return;
            }

            this.configurer.Configure();
        }

        /// <summary>
        /// Shuts down log4net.
        /// </summary>
        public void Shutdown()
        {
            LogManager.Shutdown();
        }

        private static ILogger Wrap(ILog log)
        {
            return new Log4NetLogger(log);
        }
    }
}