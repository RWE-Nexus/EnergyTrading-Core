using System;
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
        private readonly string appFabricCacheName;
        private readonly IList<Uri> appFabricUris = new List<Uri>();
        private Lazy<IDataCache> dataCache;

        public AppFabricCacheRepository(string appFabricCacheName, Uri[] appFabricUris)
        {
            this.appFabricCacheName = Validate("Appfabric cache name", appFabricCacheName, (a) => !string.IsNullOrEmpty(a));
            this.appFabricUris = appFabricUris??new Uri[0];
            dataCache = new Lazy<IDataCache>(() => GetCache(appFabricCacheName));
        }

        public AppFabricCacheRepository(string appFabricCacheName)
            : this(appFabricCacheName, null)
        {
        }

        private IList<DataCacheServerEndpoint> GetEndPoints()
        {
            return (appFabricUris.Count > 0) ? appFabricUris.Select(a => new DataCacheServerEndpoint(a.Host, a.Port)).ToList() : new List<DataCacheServerEndpoint>();
        }

        private IDataCache GetCache(string cacheName)
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
            return new AppFabricDataCache(cachefactory.GetCache(cacheName));
        }

        public ICacheService GetNamedCache(string regionName)
        {
            return new AppFabricCacheService(appFabricCacheName, regionName, dataCache.Value);
        }

        public bool RemoveNamedCache(string regionName)
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
    }
}
