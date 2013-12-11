namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Base implementation of <see cref="IMapper{T,U}" />.
    /// </summary>
    /// <typeparam name="TSource">Type of the source</typeparam>
    /// <typeparam name="TDestination">Type of the destination</typeparam>
    public abstract class SimpleMapper<TSource, TDestination> : IMapper<TSource, TDestination>
    {
        /// <copydocfrom cref="IMapper{T, D}.Map(T)" />
        /// <remarks>Override <see cref="DefaultDestination" /> to affect behaviour if source is default/null</remarks>
        public virtual TDestination Map(TSource source)
        {
            // Safe way of comparing to null/default
            if (Equals(source, default(TSource)))
            {
                return DefaultDestination();
            }

            var destination = CreateDestination();
            Map(source, destination);

            return destination;
        }

        /// <copydocfrom cref="IMapper{T, D}.Map(T, D)" />
        public abstract void Map(TSource source, TDestination destination);

        /// <summary>
        /// Creates an instance of the destination.
        /// </summary>
        /// <returns>A new instance of the destination type.</returns>
        protected abstract TDestination CreateDestination();

        /// <summary>
        /// Return a default instance of the destination.
        /// </summary>
        /// <returns>A default instance of the destination type.</returns>
        protected virtual TDestination DefaultDestination()
        {
            return default(TDestination);
        }
    }
}