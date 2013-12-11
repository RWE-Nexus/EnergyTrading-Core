namespace EnergyTrading.Registrars
{
    using System;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;

    /// <summary>
    /// Base implementation of a mapper registrar for creating versioned <see cref="IMappingEngine" />s.
    /// </summary>
    public abstract class VersionedMappingEngineRegistrar : MappingEngineRegistrar<IMappingEngine>
    {
        /// <summary>
        /// Gets the base type of the mapper, <see cref="IXmlMapper{T, D}" />
        /// </summary>
        protected override Type MapperType
        {
            get { return typeof(IMapper<,>); }
        }

        /// <inheritdoc />
        protected override IMappingEngine CreateEngine(IUnityContainer container)
        {
            var locator = container.Resolve<IServiceLocator>();
            var engine = new SimpleMappingEngine(locator) { CacheMappers = CacheMappers };

            return engine;
        }

        /// <inheritdoc />
        protected override void ParentRegister(IUnityContainer container, Version version, IMappingEngine engine)
        {
            container.RegisterInstance(ToVersionString(version), engine);
        }
    }
}