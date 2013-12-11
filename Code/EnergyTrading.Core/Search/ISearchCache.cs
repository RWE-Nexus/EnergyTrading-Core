namespace EnergyTrading.Search
{
    public interface ISearchCache
    {
        void Add(string cacheKey, SearchResult cacheItem);

        CacheSearchResultPage Get(string cacheKey, int pageNumber);

        void Clear();
    }
}