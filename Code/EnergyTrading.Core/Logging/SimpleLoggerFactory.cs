namespace EnergyTrading.Logging
{
    using System;

    /// <summary>
    /// Simple ILoggerFactory that just returns the same logger
    /// </summary>
    public class SimpleLoggerFactory : ILoggerFactory
    {
        private readonly ILogger logger;

        public SimpleLoggerFactory(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public ILogger GetLogger(string name)
        {
            return logger;
        }

        /// <inheritdoc />
        public ILogger GetLogger(Type type)
        {
            return logger;
        }

        /// <inheritdoc />
        public ILogger GetLogger<T>()
        {
            return logger;
        }

        /// <inheritdoc />
        public void Initialize()
        {
        }

        /// <inheritdoc />
        public void Shutdown()
        {            
        }
    }
}