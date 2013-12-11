namespace EnergyTrading.Mapping
{
    using System.Collections.Generic;

    /// <summary>
    /// Generic interface for mapping an object from a source to a destination.
    /// </summary>
    public interface IMappingEngine
    {
        /// <summary>
        /// Provides a context for the mapping.
        /// </summary>
        Context Context { get; set; }

        /// <summary>
        /// Map the source to a created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TDestination">Type of the destination</typeparam>
        /// <param name="source">Object to map from</param>
        /// <returns>Generated object with values mapped from the source</returns>
        TDestination Map<TSource, TDestination>(TSource source);

        /// <summary>
        /// Map a enumeration of sources to the created destination.
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TDestination">Type of the destination</typeparam>
        /// <param name="source">Enumerable objects to map from</param>
        /// <returns>Enumerable generated object with values mapped from the source</returns>
        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);

        /// <summary>
        /// Map the source to the destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TDestination">Type of the destination</typeparam>
        /// <param name="source">Object to map from</param>
        /// <param name="destination">Object to map to</param>
        void Map<TSource, TDestination>(TSource source, TDestination destination);
        
        /// <summary>
        /// Register an IMapper to use
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TDestination">Type of the destination</typeparam>
        /// <param name="mapper">The mapper to register</param>
        /// <param name="name">Name to register the mapper against</param>
        void RegisterMap<TSource, TDestination>(IMapper<TSource, TDestination> mapper, string name = null);
    }
}