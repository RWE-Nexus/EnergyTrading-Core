using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace EnergyTrading.Caching.InMemory
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheName"></param>
        public InMemoryCacheService(string cacheName)
        {
            this.cache = new MemoryCache(cacheName);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void Add<T>(string key, T value, CacheItemPolicy policy)
        {
            cache.Add(key, value, policy);
        }

        public T Get<T>(string key)
        {
            return (T)cache.Get(key);
        }
    }
}
