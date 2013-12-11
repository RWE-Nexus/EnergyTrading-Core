namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Base implementation of <see cref="IMapper{T,U}" />.
    /// </summary>
    /// <typeparam name="TSource">Type of the source.</typeparam>
    /// <typeparam name="TDestination">Type of the destination</typeparam>
    public abstract class Mapper<TSource, TDestination> : SimpleMapper<TSource, TDestination>
        where TDestination : new()
    {
        private IMappingEngine engine;

        /// <summary>
        /// Creates a new instance of the <see cref="Mapper{T, U}" /> class.
        /// </summary>
        protected Mapper() : this(null)
        {            
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Mapper{T, U}" /> class.
        /// </summary>
        /// <param name="engine">Mapping engine used to map components</param>
        protected Mapper(IMappingEngine engine)
        {
            Engine = engine;
        }

        /// <summary>
        /// Gets the mapping engine to use.
        /// </summary>
        /// <remarks>Uses a null object pattern if not set.</remarks>
        protected IMappingEngine Engine
        {
            get { return engine ?? (engine = new NullXmlMappingEngine()); }
            private set { engine = value; }
        }

        /// <summary>
        /// Creates a new instance of the target object.
        /// </summary>
        /// <returns>A new instance of the <typeparamref name="TDestination"/> object.</returns>
        protected override TDestination CreateDestination()
        {
            return new TDestination();
        }
    }
}