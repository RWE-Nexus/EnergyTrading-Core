using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using EnergyTrading.Configuration;
using Microsoft.ApplicationServer.Caching;

namespace EnergyTrading.Caching.AppFabric.Cache
{
    public class AppFabricCacheService : ICacheService
    {
        private readonly string cacheName;
        private readonly IDataCache appFabricCache;
        private readonly string regionName;

        public AppFabricCacheService(string cacheName, string regionName, IDataCache dataCache)
        {
            this.cacheName = cacheName;
            this.regionName = regionName;
            this.appFabricCache = dataCache;
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
        /// Using {regionName}-{key} to overcome creation of region in Appfabric. 
        /// Region: Items stored in a region are kept together and hence may overload a perticular node.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetFormatedKey(string key)
        {
            return string.Format("{0}-{1}", regionName, key);
        }

        public virtual void Remove(string key)
        {
            appFabricCache.Remove(GetFormatedKey(key));
        }

        public virtual void Add<T>(string key, T value, CacheItemPolicy policy = null)
        {
            Add(GetFormatedKey(key), value, policy, null);
        }

        public virtual T Get<T>(string key)
        {
            return (T)Get<T>(GetFormatedKey(key), null);
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

        protected void Remove(string key, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                appFabricCache.Remove(key);
            }
            else
            {
                appFabricCache.Remove(key, region);
            }
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
                    appFabricCache.Put<T,DataCacheItemVersion>(key, value, region);
                }
            }
        }

    }
}
