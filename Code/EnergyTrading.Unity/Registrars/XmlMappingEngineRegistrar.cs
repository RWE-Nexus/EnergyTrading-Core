namespace EnergyTrading.Registrars
{
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;

    /// <summary>
    /// Provides for default resolution behaviour for a <see cref="IXmlMappingEngine" />.
    /// <para>
    /// For cases where multiple/versioned <see cref="IXmlMappingEngine" />s are required,
    /// you should inherit <see cref="VersionedXmlMappingEngineRegistrar" />
    /// </para>
    /// </summary>
    public class XmlMappingEngineRegistrar : IContainerRegistrar
    {
        /// <copydocfrom cref="IContainerRegistrar.Register" />
        public void Register(IUnityContainer container)
        {
            // Check to see if we've already done this.
            var factory = container.TryResolve<IXmlMapperFactory>("locator");
            if (factory != null)
            {
                return;
            }

            container.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>("locator");
            container.RegisterType<IXmlMapperFactory, CachingXmlMapperFactory>(
                new PerResolveLifetimeManager(),  // So we inject a different cache into each IXmlMappingEngine
                new InjectionConstructor(new ResolvedParameter<IXmlMapperFactory>("locator")));

            container.RegisterType<IXmlMappingEngine, XmlMappingEngine>(
                new InjectionConstructor(new ResolvedParameter<IXmlMapperFactory>()));
        }
    }
}