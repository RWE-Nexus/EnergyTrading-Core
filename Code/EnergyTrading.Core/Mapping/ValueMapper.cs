namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Base implementation of IMapper
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public abstract class ValueMapper<TSource, TDestination> : IMapper<TSource, TDestination>
    {
        public abstract TDestination Map(TSource source);

        /// <inheritdoc />
        public virtual void Map(TSource source, TDestination destination)
        {
            throw new NotSupportedException("Cannot use this form of Map with ValueMapper");
        }
    }
}