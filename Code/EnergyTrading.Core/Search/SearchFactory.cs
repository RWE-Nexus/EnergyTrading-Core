namespace EnergyTrading.Search
{
    using System.Collections.Generic;

    using EnergyTrading.Contracts.Search;

    public static class SearchFactory
    {
        public static Search SimpleSearch(string property, SearchCondition op, string value)
        {
            IList<SearchCriteria> criterias = new List<SearchCriteria>
            {
                new SearchCriteria
                {
                    Combinator = SearchCombinator.And,
                    Criteria = new List<Criteria>
                    {
                        new Criteria { Field = property, Condition = op, ComparisonValue = value, },
                    }
                }
            };

            var search = new Search
            {
                SearchFields = new SearchFields { Combinator = SearchCombinator.And, Criterias = criterias },
                SearchOptions = new SearchOptions()
            };

            return search;
        }
    }
}
