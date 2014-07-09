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
        private readonly Uri appFabricUri;

        public AppFabricCacheRepository(string appFabricCacheName, Uri appFabricUri)
        {
            this.appFabricCacheName =Validate("Appfabric cache name", appFabricCacheName,(a)=>!string.IsNullOrEmpty(a));
            this.appFabricUri = Validate("Appfabric Uri",appFabricUri,(a)=>a!=null);
        }

        private IDataCache GetCache(string cacheName, Uri appfabricCacheUri)
        {
            var servers = new[] { new DataCacheServerEndpoint(appfabricCacheUri.Host, appfabricCacheUri.Port) };
            var factoryConfig = new DataCacheFactoryConfiguration
            {
                Servers = servers,
                DataCacheServiceAccountType = DataCacheServiceAccountType.DomainAccount
            };
            return new AppFabricDataCache(new DataCacheFactory(factoryConfig).GetCache(cacheName));
        }

        public ICacheService GetNamedCache(string regionName)
        {
            return new AppFabricCacheService(appFabricCacheName, regionName, GetCache(appFabricCacheName, appFabricUri));
        }

        public bool RemoveNamedCache(string regionName)
        {
            //Since Appfabric internally manages cache items based on expiration policy
            return true;
        }

        private static T Validate<T>(string name, T value, Func<T,bool> validator)
        {
            if (!validator(value))
            {
                throw new ArgumentException(string.Format("{0} parameter is missing\\empty.", name));
            }
            return value;
        }
    }
}
