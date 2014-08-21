using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace EnergyTrading.Caching.InMemory
{
    public class InMemoryCacheService : ICacheService, IDisposable
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

        public void ClearCache()
        {
            Parallel.ForEach(this.cache.Select(a => a.Key), key => cache.Remove(key));
        }


        public bool Remove(string key)
        {
            return  cache.Remove(key)!=null;
        }

        public void Add<T>(string key, T value, CacheItemPolicy policy)
        {
            cache.Add(key, value, policy);
        }

        public T Get<T>(string key)
        {
            return (T)cache.Get(key);
        }

        public void Dispose()
        {
            cache.Dispose();
        }

    }
}
