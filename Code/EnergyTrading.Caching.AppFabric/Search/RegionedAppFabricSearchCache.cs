namespace EnergyTrading.Caching.AppFabric.Search
{
    using System;
    using System.Linq;

    using Microsoft.ApplicationServer.Caching;

    using EnergyTrading.Caching;
    using EnergyTrading.Search;

    public class RegionedAppFabricSearchCache : ISearchCache
    {
        private DataCache DataCache { get; set; }
        private ICacheItemPolicyFactory CacheItemPolicyFactory { get; set; }
        private string CacheRegionName { get; set; }

        public RegionedAppFabricSearchCache(DataCache dataCache, ICacheItemPolicyFactory cacheItemPolicyFactory, string cacheRegionName)
        {
            if (dataCache == null) { throw new ArgumentNullException("dataCache"); }
            if (cacheItemPolicyFactory == null) { throw new ArgumentNullException("cacheItemPolicyFactory"); }
            if (cacheRegionName == null) { throw new ArgumentNullException("cacheRegionName"); }
            
            this.DataCache = dataCache;
            this.CacheItemPolicyFactory = cacheItemPolicyFactory;
            this.CacheRegionName = cacheRegionName;

            if (string.IsNullOrEmpty(CacheRegionName))
            {
                throw new ArgumentOutOfRangeException(cacheRegionName, "A non-empty CacheRegionName must be provided");
            }

            if (!DataCache.CreateRegion(CacheRegionName))
            {
                DataCache.ClearRegion(CacheRegionName);
            }
        }

        public void Add(string cacheKey, SearchResult cacheItem)
        {
            try
            {
                DataCache.Add(cacheKey, cacheItem, DetermineTimeOut(), CacheRegionName);
            }
            catch (DataCacheException e)
            {
                // swallow this for now, to stay consistent with how the local MemoryCache was working
                // could look at using Put instead to add or replace
                if (e.ErrorCode != DataCacheErrorCode.KeyAlreadyExists) 
                {
                    throw;
                }
            }
        }

        public CacheSearchResultPage Get(string cacheKey, int pageNumber)
        {
            var searchResult = DataCache.Get(cacheKey, CacheRegionName) as SearchResult;

            if (searchResult == null)
            {
                return null;
            }

            if (!searchResult.MultiPage)
            {
                if (pageNumber != 1)
                {
                    return null;
                }

                return new CacheSearchResultPage(searchResult.EntityIds, searchResult.AsOf, null, cacheKey);
            }

            if (searchResult.EntityIds.Count < (pageNumber * searchResult.PageSize) - searchResult.PageSize)
            {
                return null;
            }

            return new CacheSearchResultPage(
                searchResult.EntityIds.Skip((pageNumber - 1) * searchResult.PageSize ?? searchResult.EntityIds.Count).Take(searchResult.PageSize ?? searchResult.EntityIds.Count).ToList(),
                searchResult.AsOf,
                searchResult.EntityIds.Count > pageNumber * searchResult.PageSize ? new int?(pageNumber + 1) : null,
                cacheKey);
        }

        public void Clear()
        {
            DataCache.ClearRegion(CacheRegionName);
        }

        private TimeSpan DetermineTimeOut()
        {
            var policy = CacheItemPolicyFactory.CreatePolicy();
            return policy.SlidingExpiration != TimeSpan.Zero
                       ? policy.SlidingExpiration
                       : policy.AbsoluteExpiration - DateTime.UtcNow;
        }
    }
}