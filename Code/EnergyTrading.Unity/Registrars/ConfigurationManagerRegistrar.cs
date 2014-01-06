namespace EnergyTrading.Registrars
{
    using EnergyTrading.Configuration;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;

    /// <summary>
    /// Registers <see cref="IConfigurationManager" /> to use <see cref="AppConfigConfigurationManager" />
    /// </summary>
    public class ConfigurationManagerRegistrar : IContainerRegistrar
    {
        /// <copydocfrom cref="IContainerRegistrar.Register" />
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IConfigurationManager, AppConfigConfigurationManager>();
        }
    }
}