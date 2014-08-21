using System;
using System.Runtime.Caching;
using EnergyTrading.Caching;
using EnergyTrading.Caching.AppFabric.Cache;

namespace EnergyTrading.UnitTest.AppFabric
{
    public class TestAppFabricClass:IDataCache
    {
        MemoryCache cache=new MemoryCache("Test");

        public bool Remove(string key)
        {
            return cache.Remove(key)!=null;
        }

        public T Get<T>(string key)
        {
            return (T)cache.Get(key);
        }

        public bool CreateRegion(string region)
        {
            return true;
        }

        public T Get<T>(string key, string region)
        {
            return (T)cache.Get(key);
        }

        public bool Remove(string key, string region)
        {
            return Remove(key);
        }

        public void ClearCache()
        {
            cache = new MemoryCache("Test");
        }

        public TR Put<T, TR>(string key, T value, TimeSpan timeout, string region) where TR : class
        {
            var abs = new AbsoluteCacheItemPolicyFactory(timeout.Seconds);
            cache.Add(key, value, abs.CreatePolicy());
            return value as TR;
        }

        public TR Put<T, TR>(string key, T value, TimeSpan timeout) where TR : class
        {
           return  Put<T, TR>(key, value, timeout, null);
        }

        public TR Put<T, TR>(string key, T value) where TR : class
        {
            return  Put<T, TR>(key, value, TimeSpan.FromSeconds(1), null);;
        }

        public TR Put<T, TR>(string key, T value, string region) where TR : class 
        {
           return Put<T, TR>(key, value, TimeSpan.FromSeconds(1));
        }
    }
}
