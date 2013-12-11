namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Creates a <see cref="IXmlMapper{T, U}" />
    /// </summary>
    public interface IXmlMapperFactory
    {
        /// <summary>
        /// Gets a <see cref="IXmlMapper{T, U}" />
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <param name="name">Optional name of the mapper</param>
        /// <returns>The mapper from source to destination</returns>
        /// <exception cref="MappingException">Thrown if the mapper is not found.</exception>
        IXmlMapper<TSource, TDestination> Mapper<TSource, TDestination>(string name = null);

        /// <summary>
        /// Gets a <see cref="IXmlMapper{T, U}" />
        /// </summary>
        /// <param name="source">Type of the source object.</param>
        /// <param name="destination">Type of the destination object</param>
        /// <param name="name">Optional name of the mapper</param>
        /// <returns>The mapper from source to destination</returns>
        /// <remarks>Has to return object as there is no appropriate base type.</remarks>
        /// <exception cref="MappingException">Thrown if the mapper is not found.</exception>        
        object Mapper(Type source, Type destination, string name = null);

        /// <summary>
        /// Register a mapper to use.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapper">The mapper to register</param>
        /// <param name="name">Optional name of the mapper</param>        
        void Register<TSource, TDestination>(IXmlMapper<TSource, TDestination> mapper, string name = null);
    }
}