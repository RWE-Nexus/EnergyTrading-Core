namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;

    using EnergyTrading.Container;

    /// <summary>
    /// Simple mapping engine, all mappers are explicitly registered or found via ServiceLocator
    /// </summary>
    public class SimpleMappingEngine : IMappingEngine, IServiceLocatorOwner
    {
        private readonly IServiceLocator locator;
        private readonly IDictionary<string, object> mappers;
        private Context context;

        /// <summary>
        /// Create a new instance of the <see cref="SimpleMappingEngine" /> class.
        /// </summary>
        /// <param name="locator"></param>
        public SimpleMappingEngine(IServiceLocator locator)
        {
            this.locator = locator;
            mappers = new Dictionary<string, object>();
            CacheMappers = false;
        }

        /// <summary>
        /// Get or sets whether we cache mappers that we resolve.
        /// </summary>
        public bool CacheMappers { get; set; }

        /// <copydocfrom cref="IMappingEngine.Context" />
        public Context Context
        {
            get { return context ?? (context = new Context()); }
            set { context = value; }
        }

        IServiceLocator IServiceLocatorOwner.Locator
        {
            get { return locator; }
        }

        /// <copydocfrom cref="IMappingEngine.Map{T, D}(T)" />
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper<TSource, TDestination>().Map(source);
        }

        /// <copydocfrom cref="IMappingEngine.Map{T, D}(IEnumerable{T})" />
        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return source.Select(Map<TSource, TDestination>);
        }

        /// <copydocfrom cref="IMappingEngine.Map{T, D}(T, D)" />
        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            // Safe way of comparing to null/default
            if (Equals(source, default(TSource)))
            {
                // Don't invoke if we don't have a source!
                return;
            }

            Mapper<TSource, TDestination>().Map(source, destination);
        }

        /// <copydocfrom cref="IMappingEngine.RegisterMap{T, D}" />
        public void RegisterMap<TSource, TDestination>(IMapper<TSource, TDestination> mapper, string name = null)
        {
            mappers[Key<TSource, TDestination>(name)] = mapper;
        }

        /// <summary>
        /// Determine the mapper to use.
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TDestination">Type of the destination</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMapper<TSource, TDestination> Mapper<TSource, TDestination>(string name = null)
        {
            return (IMapper<TSource, TDestination>)Mapper(typeof(TSource), typeof(TDestination), name);
        }

        /// <summary>
        /// Determine the mapper to use
        /// </summary>
        /// <param name="source">Type of the source</param>
        /// <param name="destination">Type of the destination</param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected object Mapper(Type source, Type destination, string name)
        {
            object value;
            if (mappers.TryGetValue(Key(source, destination, name), out value))
            {
                return value;
            }

            var type = typeof(IMapper<,>).MakeGenericType(new[] { source, destination });

            try
            {
                var mapper = locator.GetInstance(type);
                if (mapper != null)
                {
                    // Cache the mapper - some serializations have 100K + calls to Mapper
                    if (CacheMappers)
                    {
                        mappers[Key(source, destination, name)] = mapper;
                    }
                    // ...and return it
                    return mapper;
                }
            }
            catch (Exception)
            {
                // Due to unity not implementing service locator correctly
            }

            throw new MappingException(string.Format("No mapper found from {0} to {1}", source.Name, destination.Name));
        }

        private static string Key<TSource, TDestination>(string name = null)
        {
            return Key(typeof(TSource), typeof(TDestination), name);
        }

        private static string Key(Type source, Type destination, string name)
        {
            return source.FullName + "|" + destination.FullName + "|" + name;
        }
    }
}