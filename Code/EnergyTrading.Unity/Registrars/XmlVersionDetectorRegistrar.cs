namespace EnergyTrading.Registrars
{
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;

    /// <summary>
    /// Registers the implementation of <see cref="IXmlVersionDetector" />.
    /// <para>
    /// This acts as a factory over all the named implementations that are registered in the container.
    /// </para>
    /// </summary>
    public class XmlVersionDetectorRegistrar : IContainerRegistrar
    {
        /// <copydocfrom cref="IContainerRegistrar.Register" />
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IXmlVersionDetector, XmlVersionDetector>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IXmlVersionDetector[]>()));
        }
    }
}
