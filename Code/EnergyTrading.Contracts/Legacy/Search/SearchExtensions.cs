namespace RWEST.Nexus.Contracts.Search
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SearchExtensions
    {
        public static string ToOperator(this SearchCondition condition)
        {
            switch (condition)
            {
                case SearchCondition.Equals:
                case SearchCondition.NumericEquals:
                    return "=";

                case SearchCondition.GreaterThan:
                    return ">";

                case SearchCondition.GreaterThanEquals:
                    return ">=";

                case SearchCondition.LessThan:
                    return "<";

                case SearchCondition.LessThanEquals:
                    return "<=";

                case SearchCondition.Contains:
                    return "contains";

                case SearchCondition.NotEquals:
                    return "!=";

                default:
                    return "?";
            }
        }

        public static SearchCombinator ToNexus(this EnergyTrading.Contracts.Search.SearchCombinator searchCombinator)
        {
            return (SearchCombinator)Enum.Parse(typeof(SearchCombinator), searchCombinator.ToString());
        }

        public static EnergyTrading.Contracts.Search.SearchCombinator FromNexus(this SearchCombinator searchCombinator)
        {
            return (EnergyTrading.Contracts.Search.SearchCombinator)Enum.Parse(typeof(EnergyTrading.Contracts.Search.SearchCombinator), searchCombinator.ToString());
        }

        public static SearchCondition ToNexus(this EnergyTrading.Contracts.Search.SearchCondition searchCondition)
        {
            return (SearchCondition) Enum.Parse(typeof(SearchCondition), searchCondition.ToString());
        }

        public static EnergyTrading.Contracts.Search.SearchCondition FromNexus(this SearchCondition searchCondition)
        {
            return (EnergyTrading.Contracts.Search.SearchCondition)Enum.Parse(typeof(EnergyTrading.Contracts.Search.SearchCondition), searchCondition.ToString());
        }

        public static Criteria ToNexus(this EnergyTrading.Contracts.Search.Criteria criteria)
        {
            return new Criteria
                       {
                           ComparisonValue = criteria.ComparisonValue,
                           Condition = criteria.Condition.ToNexus(),
                           Field = criteria.Field,
                           IsNumeric = criteria.IsNumeric
                       };
        }

        public static EnergyTrading.Contracts.Search.Criteria FromNexus(this Criteria criteria)
        {
            return new EnergyTrading.Contracts.Search.Criteria
            {
                ComparisonValue = criteria.ComparisonValue,
                Condition = criteria.Condition.FromNexus(),
                Field = criteria.Field,
                IsNumeric = criteria.IsNumeric
            };
        }

        public static SearchOptions ToNexus(this EnergyTrading.Contracts.Search.SearchOptions searchOptions)
        {
            return new SearchOptions
                       {
                           CaseSensitivity = searchOptions.CaseSensitivity,
                           IsMappingSearch = searchOptions.IsMappingSearch,
                           MultiPage = searchOptions.MultiPage,
                           OrderBy = searchOptions.OrderBy,
                           ResultsPerPage = searchOptions.ResultsPerPage
                       };
        }

        public static EnergyTrading.Contracts.Search.SearchOptions FromNexus(this SearchOptions searchOptions)
        {
            return new EnergyTrading.Contracts.Search.SearchOptions
            {
                CaseSensitivity = searchOptions.CaseSensitivity,
                IsMappingSearch = searchOptions.IsMappingSearch,
                MultiPage = searchOptions.MultiPage,
                OrderBy = searchOptions.OrderBy,
                ResultsPerPage = searchOptions.ResultsPerPage
            };
        }

        public static Search ToNexus(this EnergyTrading.Contracts.Search.Search search)
        {
            return new Search
                       {
                           AsOf = search.AsOf,
                           SearchFields = search.SearchFields.ToNexus(),
                           SearchOptions = search.SearchOptions.ToNexus()
                       };
        }

        public static EnergyTrading.Contracts.Search.Search FromNexus(this Search search)
        {
            return new EnergyTrading.Contracts.Search.Search
            {
                AsOf = search.AsOf,
                SearchFields = search.SearchFields.FromNexus(),
                SearchOptions = search.SearchOptions.FromNexus()
            };
        }

        public static SearchCriteria ToNexus(this EnergyTrading.Contracts.Search.SearchCriteria criteria)
        {
            var c = new SearchCriteria
                       {
                           Criteria = new List<Criteria>(),
                           Combinator = criteria.Combinator.ToNexus()
                       };
            criteria.Criteria.ForEach(x => c.Criteria.Add(x.ToNexus()));
            return c;
        }

        public static EnergyTrading.Contracts.Search.SearchCriteria FromNexus(this SearchCriteria criteria)
        {
            var c = new EnergyTrading.Contracts.Search.SearchCriteria
            {
                Criteria = new List<EnergyTrading.Contracts.Search.Criteria>(),
                Combinator = criteria.Combinator.FromNexus()
            };
            criteria.Criteria.ForEach(x => c.Criteria.Add(x.FromNexus()));
            return c;
        }

        public static SearchFields ToNexus(this EnergyTrading.Contracts.Search.SearchFields searchFields)
        {
            var c = new SearchFields
                       {
                           Criterias = new List<SearchCriteria>(),
                           Combinator = searchFields.Combinator.ToNexus()
                       };
            searchFields.Criterias.ToList().ForEach(x => c.Criterias.Add(x.ToNexus()));
            return c;
        }

        public static EnergyTrading.Contracts.Search.SearchFields FromNexus(this SearchFields searchFields)
        {
            var c = new EnergyTrading.Contracts.Search.SearchFields
            {
                Criterias = new List<EnergyTrading.Contracts.Search.SearchCriteria>(),
                Combinator = searchFields.Combinator.FromNexus()
            };
            searchFields.Criterias.ToList().ForEach(x => c.Criterias.Add(x.FromNexus()));
            return c;
        }
    }   
}