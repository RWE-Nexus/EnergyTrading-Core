namespace EnergyTrading.Mapping
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    using EnergyTrading.Container;

    /// <summary>
    /// Implementation of <see cref="IXmlMapperFactory" /> that uses <see cref="IServiceLocator" />
    /// </summary>
    public class LocatorXmlMapperFactory : IXmlMapperFactory, IServiceLocatorOwner
    {
        private readonly IServiceLocator locator;

        /// <summary>
        /// Create a new instance of the <see cref="LocatorXmlMapperFactory" /> class.
        /// </summary>
        /// <param name="locator"></param>
        public LocatorXmlMapperFactory(IServiceLocator locator)
        {
            this.locator = locator;
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper{T, U}" />
        public IXmlMapper<TSource, TDestination> Mapper<TSource, TDestination>(string name = null)
        {
            return (IXmlMapper<TSource, TDestination>)Mapper(typeof(TSource), typeof(TDestination), name);
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper" />
        public object Mapper(Type source, Type destination, string name = null)
        {
            var type = typeof(IXmlMapper<,>).MakeGenericType(new[] { source, destination });

            try
            {
                var mapper = this.locator.GetInstance(type, name);
                if (mapper != null)
                {
                    // ...and return it
                    return mapper;
                }
            }
            catch (Exception)
            {
                // Due to unity not implementing service locator correctly
            }

            throw new MappingException(string.Format("No xml mapper found from {0} to {1}", source.Name, destination.Name));
        }

        /// <contentfrom cref="IXmlMapperFactory.Register{T, U}" />
        public void Register<TSource, TDestination>(IXmlMapper<TSource, TDestination> mapper, string name = null)
        {
            throw new NotSupportedException("Use CachingMapperFactory if registration required.");
        }

        IServiceLocator IServiceLocatorOwner.Locator
        {
            get { return this.locator; }
        }
    }
}