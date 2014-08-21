using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace EnergyTrading.Caching.AppFabric.Cache
{
    /// <summary>
    /// This interface helps to Unit test 
    /// AppfabricCacheService & AppFabricCacheRespository without appFabric setup
    /// </summary>
   public interface IDataCache
   {
       bool Remove(string key);
       T Get<T>(string key);
       bool CreateRegion(string region);
       T Get<T>(string key, string region);
       bool Remove(string key, string region);
       void ClearCache();
       TReturn Put<T, TReturn>(string key, T value, TimeSpan timeout, string region) where TReturn : class;
       TReturn Put<T, TReturn>(string key, T value, TimeSpan timeout) where TReturn : class;
       TReturn Put<T, TReturn>(string key, T value) where TReturn : class;
       TReturn Put<T, TReturn>(string key, T value, string region) where TReturn : class;
   }
}
