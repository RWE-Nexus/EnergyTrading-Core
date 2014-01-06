namespace EnergyTrading.Logging.EnterpriseLibrary
{
    using System;

    using EnergyTrading.Logging;

    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class EntLibLoggerFactory : ILoggerFactory
    {
        private ILogger logger;

        public ILogger GetLogger(string name)
        {
            return this.logger;
        }

        public ILogger GetLogger(Type type)
        {
            return this.logger;
        }

        public ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T).FullName);
        }

        public void Initialize()
        {
            this.logger = new EntLibLogger(Logger.Writer);
        }

        public void Shutdown()
        {            
        }
    }
}