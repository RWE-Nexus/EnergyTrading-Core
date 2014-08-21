using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnergyTrading.Logging;
using Microsoft.ApplicationServer.Caching;

namespace EnergyTrading.Caching.AppFabric.Cache
{
    internal class AppFabricDataCache : IDataCache
    {
        private readonly Func<DataCache> dataCacheFunc;
        DataCache appfabricDataCache;
        private static readonly ILogger Logger = LoggerFactory.GetLogger<AppFabricDataCache>();
        private DateTime delayAppfabricCall = DateTime.UtcNow;

        public AppFabricDataCache(Func<DataCache> dataCacheFunc)
        {
            this.dataCacheFunc = dataCacheFunc;            
        }

        private TReturn HandleError<TReturn>(Func<DataCache, TReturn> appFabricOperation, TReturn toReturnOnFailure)
        {
            try
            {
                if (delayAppfabricCall > DateTime.UtcNow)
                {
                    Logger.DebugFormat("Skipping appfabric call till {0}.", delayAppfabricCall);
                    return toReturnOnFailure;
                }

                if (appfabricDataCache == null)
                {
                    lock (dataCacheFunc)
                    {
                        if (appfabricDataCache == null)
                        {
                            appfabricDataCache = dataCacheFunc();
                        }
                    }
                }

                delayAppfabricCall = DateTime.UtcNow;
                return appFabricOperation(appfabricDataCache);
            }
            catch (DataCacheException exp)
            {
                Logger.Error("Error calling Appfabric", exp);
                if (exp.ErrorCode == DataCacheErrorCode.RetryLater)
                {
                    delayAppfabricCall = DateTime.UtcNow.AddSeconds(30);
                }
            }
            catch (AggregateException exp)
            {
                Logger.Error("Error calling Appfabric", exp.Flatten());
            }
            catch (Exception exp)
            {
                Logger.Error("Error calling Appfabric",exp);
            }
            return toReturnOnFailure;
        }

        public bool Remove(string key)
        {
            return HandleError(dataCache=>dataCache.Remove(key),false);
        }

        public T Get<T>(string key)
        {
            return (T)HandleError(dataCache=>dataCache.Get(key),default(T));
        }

        public bool CreateRegion(string region)
        {
            return HandleError(dataCache=>dataCache.CreateRegion(region),false);
        }

        public T Get<T>(string key, string region)
        {
            return (T)HandleError(dataCache=>dataCache.Get(key, region),default(T));
        }

        public bool Remove(string key, string region)
        {
            return HandleError(dataCache=>dataCache.Remove(key, region),false);
        }

        public TReturn Put<T, TReturn>(string key, T value, TimeSpan timeout, string region) where TReturn : class
        {
            return HandleError(dataCache => dataCache.Put(key, value, timeout, region) as TReturn, default(TReturn));
        }

        public TReturn Put<T, TReturn>(string key, T value, TimeSpan timeout) where TReturn : class
        {
            return HandleError(dataCache => dataCache.Put(key, value, timeout) as TReturn,default(TReturn));
        }

        public TReturn Put<T, TReturn>(string key, T value) where TReturn : class
        {
            return HandleError(dataCache => dataCache.Put(key, value) as TReturn,default(TReturn));
        }

        public TReturn Put<T, TReturn>(string key, T value, string region) where TReturn : class
        {
            return HandleError(dataCache => dataCache.Put(key, value, region) as TReturn,default(TReturn));
        }

        public void ClearCache()
        {
            HandleError<bool>(dataCache =>
                        {
                            Parallel.ForEach(dataCache.GetSystemRegions(), region =>
                                                                           {
                                                                               dataCache.ClearRegion(region);
                                                                               var sysRegion =
                                                                                   dataCache.GetSystemRegionName(region);
                                                                               dataCache.ClearRegion(sysRegion);
                                                                           });
                            return true;
                        },false);


        }
    }
}
