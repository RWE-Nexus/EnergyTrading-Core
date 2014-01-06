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
            return this.logger;
        }

        /// <inheritdoc />
        public ILogger GetLogger(Type type)
        {
            return this.logger;
        }

        /// <inheritdoc />
        public ILogger GetLogger<T>()
        {
            return this.logger;
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