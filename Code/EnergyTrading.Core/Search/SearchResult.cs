namespace EnergyTrading.Search
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SearchResult
    {
        public SearchResult(IList<int> ids, DateTime asOf, bool multiPage, int? pageSize)
        {
            this.EntityIds = ids;
            this.AsOf = asOf;
            this.MultiPage = multiPage;
            this.PageSize = pageSize;
        }

        public DateTime AsOf { get; private set; }

        public bool MultiPage { get; private set; }

        public IList<int> EntityIds { get; private set; }

        public int? PageSize { get; private set; }
    }
}