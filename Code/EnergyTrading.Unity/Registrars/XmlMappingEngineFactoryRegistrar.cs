namespace EnergyTrading.Registrars
{
    using System;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;

    /// <summary>
    /// Register up a <see cref="IXmlMappingEngineFactory" />
    /// </summary>
    public class XmlMappingEngineFactoryRegistrar : IContainerRegistrar
    {
        /// <copydocfrom cref="IContainerRegistrar.Register" />
        public void Register(IUnityContainer container)
        {
            // Check to see if we've already done this.
            var factory = container.TryResolve<IXmlMappingEngineFactory>("locator");
            if (factory != null)
            {
                return;
            }

            // Should be there before here.
            var locator = container.TryResolve<IServiceLocator>();
            if (locator == null)
            {
                throw new NotSupportedException("Must set up container with IServiceLocator");
            }

            container.RegisterType<IXmlMappingEngineFactory, LocatorXmlMappingEngineFactory>("locator");

            container.RegisterType<IXmlMappingEngineFactory, CompatibleXmlMappingEngineFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<IXmlMappingEngineFactory>("locator"),
                    new ResolvedParameter<IXmlSchemaRegistry>()));
        }
    }
}