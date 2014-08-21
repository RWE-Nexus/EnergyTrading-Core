using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnergyTrading.Caching.InMemory
{
    public class InMemoryCacheRepository : ICacheRepository
    {
        private readonly ConcurrentDictionary<string, InMemoryCacheService> cacheServiceCollection;


        public InMemoryCacheRepository()
        {
            this.cacheServiceCollection = new ConcurrentDictionary<string, InMemoryCacheService>(StringComparer.InvariantCultureIgnoreCase);
        }

        public ICacheService GetNamedCache(string cacheName)
        {
            var cacheObj = cacheServiceCollection.GetOrAdd(cacheName, new InMemoryCacheService(cacheName));
            return cacheObj;
        }

        public bool ClearNamedCache(string cacheName)
        {
            if (!cacheServiceCollection.ContainsKey(cacheName)) return false;
            var cache = cacheServiceCollection[cacheName];
            if (cache == null) return false;
            cache.ClearCache();
            return true;

        }

        public bool RemoveNamedCache(string cacheName)
        {
            if (!cacheServiceCollection.ContainsKey(cacheName)) return false;
            InMemoryCacheService cacheToRemove;
            cacheServiceCollection.TryRemove(cacheName, out cacheToRemove);
            if (cacheToRemove == null) return false;
            cacheToRemove.Dispose();
            return true;

        }
    }
}
