using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EnergyTrading.Configuration;
using Microsoft.ApplicationServer.Caching;


namespace EnergyTrading.Caching.AppFabric.Cache
{
    public class AppFabricCacheRepository : ICacheRepository
    {
        private readonly string defaultAppFabricCacheName;
        private readonly IList<Uri> appFabricUris = new List<Uri>();
        private readonly ConcurrentDictionary<string, IDataCache> cacheServiceCollection;
        private readonly IConfigurationManager configuration;

        public AppFabricCacheRepository(string appFabricCacheName, Uri[] appFabricUris, IConfigurationManager configuration)
        {
            this.defaultAppFabricCacheName = Validate("Appfabric cache name", appFabricCacheName, (a) => !string.IsNullOrEmpty(a));
            this.appFabricUris = appFabricUris ?? new Uri[0];
            this.configuration = configuration;
            cacheServiceCollection = new ConcurrentDictionary<string, IDataCache>(StringComparer.InvariantCultureIgnoreCase);
        }

        public AppFabricCacheRepository(string appFabricCacheName, IConfigurationManager configuration)
            : this(appFabricCacheName, null, configuration)
        {
        }

        private IList<DataCacheServerEndpoint> GetEndPoints()
        {
            return (appFabricUris.Count > 0) ? appFabricUris.Select(a => new DataCacheServerEndpoint(a.Host, a.Port)).ToList() : new List<DataCacheServerEndpoint>();
        }

        private DataCacheFactory GetDataCacheFactory()
        {
            var servers = GetEndPoints();
            DataCacheFactory cachefactory;
            if (servers.Count > 0)
            {
                var factoryConfig = new DataCacheFactoryConfiguration
                {
                    Servers = servers,
                    DataCacheServiceAccountType = DataCacheServiceAccountType.DomainAccount
                };
                cachefactory = new DataCacheFactory(factoryConfig);
            }
            else
            {
                cachefactory = new DataCacheFactory();
            }

            return cachefactory;
        }

        private IDataCache GetCache(string cacheName)
        {
            return cacheServiceCollection.GetOrAdd(cacheName, (a) =>new AppFabricDataCache(()=>GetDataCacheFactory().GetCache(a)));
            //new AppFabricDataCache(() => cacheServiceCollection.GetOrAdd(cacheName, (a) => GetDataCacheFactory().GetCache(a)));
        }

        private IDataCache ResolveAppfabricCacheFromNamedCache(string namedCache)
        {
            var nonDefaultCacheName = configuration.AppSettings[string.Format("NamedCache.{0}.AppFabricCacheName", namedCache)];
            return GetCache(!string.IsNullOrWhiteSpace(nonDefaultCacheName) ? nonDefaultCacheName : defaultAppFabricCacheName);
        }

        /// <summary>
        /// By default items cached using the region name would be stored in named cache configured in "AppFabricCacheName".
        /// If items stored using specific region name should be stored in different named cache then it can be configured
        /// as NamedCache.{namedCache}.AppFabricCacheName.
        /// </summary>
        /// <param name="namedCache"></param>
        /// <returns></returns>
        public ICacheService GetNamedCache(string namedCache)
        {
            return new AppFabricCacheService(defaultAppFabricCacheName, namedCache, ResolveAppfabricCacheFromNamedCache(namedCache));
        }

        public bool RemoveNamedCache(string namedCache)
        {
            //Since Appfabric internally manages cache items based on expiration policy
            return true;
        }

        private static T Validate<T>(string name, T value, Func<T, bool> validator)
        {
            if (!validator(value))
            {
                throw new ArgumentException(string.Format("{0} parameter is missing\\empty.", name));
            }
            return value;
        }

        public bool ClearNamedCache(string namedCacheName)
        {
            if (string.IsNullOrWhiteSpace(namedCacheName)) return false;

            ResolveAppfabricCacheFromNamedCache(namedCacheName).ClearCache();

            return true;
        }

    }
}
