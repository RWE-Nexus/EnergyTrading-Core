namespace EnergyTrading.Data
{
    using EnergyTrading.Contracts.Search;

    /// <summary>
    /// Converts searches into executable queries.
    /// </summary>
    public interface IQueryFactory
    {
        /// <summary>
        /// Create a query.
        /// </summary>
        /// <param name="search">Search to use</param>
        /// <returns>Query string executable by the engine.</returns>
        string CreateQuery(Search search);
    }
}