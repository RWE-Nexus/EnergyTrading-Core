namespace EnergyTrading.Search
{
    using System;
    using System.Collections.Generic;

    public class CacheSearchResultPage
    {
        public CacheSearchResultPage(IList<int> ids, DateTime asOf, int? nextPage, string searchResultsKey)
        {
            this.EntityIds = ids;
            this.AsOf = asOf;
            this.SearchResultsKey = searchResultsKey;
            this.NextPage = nextPage;
        }

        public DateTime AsOf { get; private set; }

        public IList<int> EntityIds { get; private set; }

        public int? NextPage { get; private set; }

        public string SearchResultsKey { get; private set; }
    }
}
