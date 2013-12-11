namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Maps an object from a source to a destination.
    /// </summary>
    /// <typeparam name="TSource">Type of the source.</typeparam>
    /// <typeparam name="TDestination">Type of the destination.</typeparam>
    public interface IMapper<in TSource, TDestination>
    {
        /// <summary>
        /// Map the source to a created destination.
        /// </summary>
        /// <param name="source">Object to map from</param>
        /// <returns>Default of <typeparamref name="TDestination"/> if source is null, otherwise generated object with values mapped from the source</returns>
        TDestination Map(TSource source);

        /// <summary>
        /// Map the source to the destination.
        /// </summary>
        /// <param name="source">Object to map from</param>
        /// <param name="destination">Object to map to</param>
        void Map(TSource source, TDestination destination);
    }
}