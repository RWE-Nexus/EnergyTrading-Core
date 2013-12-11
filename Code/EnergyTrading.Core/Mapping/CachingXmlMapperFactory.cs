namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Implementation of <see cref="IXmlMapperFactory" /> that caches results.
    /// </summary>
    public class CachingXmlMapperFactory : IXmlMapperFactory
    {
        private readonly IXmlMapperFactory factory;
        private readonly ConcurrentDictionary<string, object> mappers;

        public CachingXmlMapperFactory(IXmlMapperFactory factory)
        {
            this.factory = factory;
            this.mappers = new ConcurrentDictionary<string, object>();
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper{T, U}" />
        public IXmlMapper<TSource, TDestination> Mapper<TSource, TDestination>(string name = null)
        {
            return (IXmlMapper<TSource, TDestination>)Mapper(typeof(TSource), typeof(TDestination), name);
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper" />
        public object Mapper(Type source, Type destination, string name = null)
        {
            var key = Key(source, destination, name);

            return mappers.GetOrAdd(key, s => factory.Mapper(source, destination, name));
        }

        /// <contentfrom cref="IXmlMapperFactory.Register{T, U}" />
        public void Register<TSource, TDestination>(IXmlMapper<TSource, TDestination> mapper, string name = null)
        {
            if (mapper == null) { throw new ArgumentNullException("mapper"); }

            var key = Key(typeof(TSource), typeof(TDestination), name);

            mappers[key] = mapper;
        }

        private static string Key(Type source, Type destination, string name = null)
        {
            return source.FullName + "|" + destination.FullName + "|" + name;
        }
    }
}