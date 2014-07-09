using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace EnergyTrading.Caching.AppFabric.Cache
{
   internal class AppFabricDataCache : IDataCache
   {
       private readonly DataCache dataCache;

       public AppFabricDataCache(DataCache dataCache)
       {
           this.dataCache = dataCache;
       }

       public bool Remove(string key)
       {
           return dataCache.Remove(key);
       }

        public T Get<T>(string key)
        {
            return (T) dataCache.Get(key);
        }

        public bool CreateRegion(string region)
        {
            return dataCache.CreateRegion(region);
        }

        public T Get<T>(string key, string region)
        {
            return (T)dataCache.Get(key,region);
        }

        public bool Remove(string key, string region)
        {
            return dataCache.Remove(key, region);
        }

        public TReturn Put<T, TReturn>(string key, T value, TimeSpan timeout, string region) where TReturn : class 
        {
            return dataCache.Put(key, value, timeout, region) as TReturn;
        }

        public TReturn Put<T, TReturn>(string key, T value, TimeSpan timeout) where TReturn : class 
        {
            return dataCache.Put(key, value, timeout) as TReturn;
        }

        public TReturn Put<T, TReturn>(string key, T value) where TReturn : class 
        {
            return dataCache.Put(key, value) as TReturn;
        }

        public TReturn Put<T, TReturn>(string key, T value, string region) where TReturn : class 
       {
           return dataCache.Put(key, value, region) as TReturn;
       }
   }
}
