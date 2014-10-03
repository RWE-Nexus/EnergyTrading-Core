using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using EnergyTrading.Configuration;
using EnergyTrading.Logging;
using Microsoft.ApplicationServer.Caching;

namespace EnergyTrading.Caching.AppFabric.Cache
{
    public class AppFabricCacheService : ICacheService
    {
        private readonly string appFabricCacheName;
        private readonly IDataCache appFabricCache;
        private readonly string namedCache;
        private static readonly ILogger Logger = LoggerFactory.GetLogger<AppFabricCacheService>();

        public AppFabricCacheService(string appFabricCacheName, string namedCache, IDataCache dataCache)
        {
            this.appFabricCacheName = appFabricCacheName;
            this.namedCache = namedCache;
            appFabricCache = dataCache;
        }

        private static TimeSpan DetermineTimeOut(CacheItemPolicy policy)
        {
            if (policy == null) return TimeSpan.Zero;
            if (!(policy.SlidingExpiration != TimeSpan.Zero))
            {
                return policy.AbsoluteExpiration - DateTime.UtcNow;
            }
            return policy.SlidingExpiration;
        }

        /// <summary>
        /// Using {namedCache}-{key} to overcome creation of region in Appfabric. 
        /// Region: Items stored in a region are kept together and hence may overload a perticular node.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetFormatedKey(string key)
        {
            return string.Format("{0}-{1}", namedCache, key);
        }
        
        public virtual bool Remove(string key)
        {
            return Remove(GetFormatedKey(key),null);
        }

        public virtual void Add<T>(string key, T value, CacheItemPolicy policy = null)
        {
            Add(GetFormatedKey(key), value, policy, null);
        }

        public virtual T Get<T>(string key)
        {
            return Get<T>(GetFormatedKey(key),null);
        }

        protected T Get<T>(string key, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                return appFabricCache.Get<T>(key);
            }

            //This function will create if region doesnt exists. Prevents exception if called before adding elements into Region
            appFabricCache.CreateRegion(region);
            return appFabricCache.Get<T>(key, region);
        }

        protected bool Remove(string key, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
               return appFabricCache.Remove(key);
            }

            return appFabricCache.Remove(key, region);
        }

        protected void Add<T>(string key, T value, CacheItemPolicy policy, string region)
        {
            if (policy != null)
            {
                var timeout = DetermineTimeOut(policy);

                if (string.IsNullOrEmpty(region))
                {
                    appFabricCache.Put<T, DataCacheItemVersion>(key, value, timeout);
                }
                else
                {
                    appFabricCache.CreateRegion(region);
                    appFabricCache.Put<T, DataCacheItemVersion>(key, value, timeout, region);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(region))
                {
                    appFabricCache.Put<T, DataCacheItemVersion>(key, value);
                }
                else
                {
                    appFabricCache.Put<T, DataCacheItemVersion>(key, value, region);
                }
            }
        }

    }
}
