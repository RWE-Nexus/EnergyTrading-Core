namespace EnergyTrading.Mapping
{
    using System.Collections.Concurrent;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Implementation of <see cref="IXmlMappingEngineFactory" /> using <see cref="IServiceLocator" />
    /// </summary>
    public class LocatorXmlMappingEngineFactory : IXmlMappingEngineFactory
    {
        private readonly IServiceLocator locator;
        private readonly ConcurrentDictionary<string, IXmlMappingEngine> engines;

        public LocatorXmlMappingEngineFactory(IServiceLocator locator)
        {
            this.locator = locator;
            this.engines = new ConcurrentDictionary<string, IXmlMappingEngine>();
        }

        /// <copydocfrom cref="IXmlMappingEngineFactory.Find" />
        public IXmlMappingEngine Find(string version)
        {
            IXmlMappingEngine engine;
            if (!this.TryFind(version, out engine))
            {
                throw new MappingException("IXmlMappingEngine not found: " + version);
            }

            return engine;
        }

        /// <copydocfrom cref="IXmlMappingEngineFactory.TryFind" />
        public bool TryFind(string version, out IXmlMappingEngine engine)
        {
            if (!engines.TryGetValue(version, out engine))
            {
                engine = this.Get(version);
                if (engine != null)
                {
                    // Cache it.
                    engines[version] = engine;
                }
            }

            return engine != null;
        }

        private IXmlMappingEngine Get(string version)
        {
            try
            {
                return locator.GetInstance<IXmlMappingEngine>(version);
            }
            catch
            {
                // NOTE: Caters for IServiceLocator implementations that throw rather than returning null
                return null;
            }  
        }
    }
}