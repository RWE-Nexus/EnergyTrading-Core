namespace EnergyTrading.Search
{
    using System.Collections.Generic;

    using EnergyTrading.Contracts.Search;

    /// <summary>
    /// Little fluent interface for creating searches
    /// </summary>
    public static class SearchBuilder
    {
        public static Search CreateSearch(SearchCombinator combinator = SearchCombinator.And, bool isMappingSearch = false)
        {
            var search = new Search();

            if (search.SearchFields == null)
            {
                search.SearchFields = new SearchFields();
            }

            search.SearchOptions.IsMappingSearch = isMappingSearch;
            search.SearchFields.Combinator = combinator;

            return search;
        }

        public static Search MaxPageSize(this Search search, int? maxResults)
        {
            search.SearchOptions.ResultsPerPage = maxResults;
            return search;
        }

        public static Search NoMaxPageSize(this Search search)
        {
            search.SearchOptions.ResultsPerPage = null;
            return search;
        }

        public static Search IsMutliPage(this Search search)
        {
            search.SearchOptions.MultiPage = true;
            return search;
        }

        public static Search NotMultiPage(this Search search)
        {
            search.SearchOptions.MultiPage = false;
            return search;
        }

        public static SearchCriteria AddCriteria(this SearchCriteria searchCriteria, string field, SearchCondition condition, string comparisonValue, bool? isNumeric = null)
        {
            if (searchCriteria.Criteria == null)
            {
                searchCriteria.Criteria = new List<Criteria>();
            }

            searchCriteria.Criteria.Add(new Criteria { ComparisonValue = comparisonValue, Condition = condition, Field = field, IsNumeric = isNumeric});

            return searchCriteria;
        }

        public static SearchCriteria AddSearchCriteria(this Search search, SearchCombinator combinator)
        {
            if (search.SearchFields == null)
            {
                search.SearchFields = new SearchFields();
            }

            if (search.SearchFields.Criterias == null)
            {
                search.SearchFields.Criterias = new List<SearchCriteria>();
            }

            var searchCriteria = new SearchCriteria();
            searchCriteria.Combinator = combinator;
            search.SearchFields.Criterias.Add(searchCriteria);

            return searchCriteria;
        }
    }
}