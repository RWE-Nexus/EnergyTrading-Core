namespace EnergyTrading.Search
{
    using System;
    using System.Collections.Generic;

    public class SearchResultPage<TContract>
    {
        public SearchResultPage(CacheSearchResultPage cache, List<TContract> contracts)
        {
            this.Contracts = contracts;
            this.AsOf = cache.AsOf;
            this.SearchResultsKey = cache.SearchResultsKey;
            this.NextPage = cache.NextPage;
        }

        public SearchResultPage(List<TContract> contracts, DateTime asOf)
        {
            this.Contracts = contracts;
            this.AsOf = asOf;
        }

        public DateTime AsOf { get; private set; }

        public IList<TContract> Contracts { get; private set; }

        public int? NextPage { get; private set; }

        public string SearchResultsKey { get; private set; }
    }
}
