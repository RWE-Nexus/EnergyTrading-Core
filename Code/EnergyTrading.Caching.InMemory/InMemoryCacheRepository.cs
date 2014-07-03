using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnergyTrading.Caching.InMemory
{
    public class InMemoryCacheRepository : ICacheRepository
    {
        private Dictionary<string, InMemoryCacheService> caches;

        public InMemoryCacheRepository()
        {
            this.caches = new Dictionary<string, InMemoryCacheService>(StringComparer.InvariantCultureIgnoreCase);
        }

        public ICacheService GetNamedCache(string cacheName)
        {
            InMemoryCacheService cacheObj;
            lock (caches)
            {
                if (!caches.TryGetValue(cacheName, out cacheObj))
                {
                    cacheObj = new InMemoryCacheService(cacheName);
                    caches.Add(cacheName, cacheObj);
                }
            }
            return cacheObj;
        }

        public bool RemoveNamedCache(string cacheName)
        {
            lock (caches)
            {
                if (caches.ContainsKey(cacheName))
                {
                    return caches.Remove(cacheName);
                }
            }
            return false;
        }
    }
}
