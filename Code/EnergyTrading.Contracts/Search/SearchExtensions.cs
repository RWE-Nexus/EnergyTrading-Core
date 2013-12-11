namespace EnergyTrading.Contracts.Search
{
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
    }
}