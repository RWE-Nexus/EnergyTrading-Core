namespace EnergyTrading.Search
{
    using System.Linq;
    using System.Runtime.Caching;
    using EnergyTrading.Caching;

    public class SearchCache : ISearchCache
    {
        private readonly ICacheItemPolicyFactory cacheItemPolicyFactory;
        private ObjectCache cache;

        public SearchCache(ICacheItemPolicyFactory cacheItemPolicyFactory)
        {
            this.cacheItemPolicyFactory = cacheItemPolicyFactory;
            this.cache = new MemoryCache("EnergyTrading.SearchCache");
        }

        public void Add(string cacheKey, SearchResult cacheItem)
        {
            cache.Add(cacheKey, cacheItem, this.cacheItemPolicyFactory.CreatePolicy());
        }

        public CacheSearchResultPage Get(string cacheKey, int pageNumber)
        {
            var searchResult = cache.Get(cacheKey) as SearchResult;

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
            cache = new MemoryCache("EnergyTrading.SearchCache");
        }
    }
}