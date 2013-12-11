namespace EnergyTrading.Data
{
    using System;
    using System.Text;

    using EnergyTrading.Contracts.Search;

    /// <copydocfrom cref="IQueryFactory" />
    public class QueryFactory : IQueryFactory
    {
        /// <copydocfrom cref="IQueryFactory.CreateQuery" />
        public string CreateQuery(Search search)
        {
            if (search.SearchFields == null || search.SearchFields.Criterias == null)
            {
                return string.Empty;
            }

            var queryBuilder = new StringBuilder();

            for (var i = 0; i < search.SearchFields.Criterias.Count; i++)
            {
                var searchcriteria = search.SearchFields.Criterias[i];
                queryBuilder.Append("(");
    
                for (var j = 0; j < searchcriteria.Criteria.Count; j++)
                {
                    var criteria = searchcriteria.Criteria[j];
                    queryBuilder.Append(criteria.Field);
                    this.AddComparisonValue(queryBuilder, criteria, search.SearchOptions);

                    if (j < searchcriteria.Criteria.Count - 1)
                    {
                        queryBuilder.Append(Combinator(searchcriteria.Combinator));
                    }
                }
                queryBuilder.Append(")");

                if (i < search.SearchFields.Criterias.Count - 1)
                {
                    queryBuilder.Append(Combinator(search.SearchFields.Combinator));
                }
            }

            return queryBuilder.ToString();
        }

        private void AddComparisonValue(StringBuilder queryBuilder, Criteria criteria, SearchOptions options)
        {
            switch(criteria.Condition)
            {
                case SearchCondition.Contains:
                    if (options.IsMappingSearch && options.CaseSensitivity == false)
                    {
                        queryBuilder.Append(".ToUpper().Contains");
                        queryBuilder.Append("(");
                        queryBuilder.Append(FormattedValue(criteria).ToUpper());
                        queryBuilder.Append(")");
                    }
                    else
                    {
                        queryBuilder.Append(".Contains");
                        queryBuilder.Append("(");
                        queryBuilder.Append(FormattedValue(criteria));
                        queryBuilder.Append(")");
                    }
                    break;

                default:
                    queryBuilder.Append(Operator(criteria.Condition));
                    queryBuilder.Append(FormattedValue(criteria));
                    break;
            }
        }

        private static string FormattedValue(Criteria criteria)
        {
            const string Quote = "\"";

            if (criteria.Condition == SearchCondition.NumericEquals || (criteria.IsNumeric.HasValue && criteria.IsNumeric == true))
            {
                return criteria.ComparisonValue;
            }

            return Quote + criteria.ComparisonValue + Quote;
        }

        private static string Operator(SearchCondition op)
        {
            switch (op)
            {
                case SearchCondition.Equals:
                case SearchCondition.NumericEquals:
                    return " = ";

                case SearchCondition.GreaterThan:
                    return " > ";

                case SearchCondition.GreaterThanEquals:
                    return " >= ";

                case SearchCondition.LessThan:
                    return " < ";

                case SearchCondition.LessThanEquals:
                    return " <= ";

                case SearchCondition.NotEquals:
                    return " != ";

                default:
                    throw new NotSupportedException("Unsupported operator: " + op);
            }
        }

        private static string Combinator(SearchCombinator combinator)
        {
            switch (combinator)
            {
                case SearchCombinator.And:
                    return " And ";

                case SearchCombinator.Or:
                    return " Or ";

                default:
                    throw new NotSupportedException("Unsupported combinator: " + combinator);
            }  
        }
    }
}