namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Configuration;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Registrars;

    [TestClass]
    public class ConfigurationManagerRegistrarFixture
    {
        [TestMethod]
        public void CanResolveIConfigurationManager()
        {
            var container = new UnityContainer();
            new ConfigurationManagerRegistrar().Register(container);
            var candidate = container.Resolve<IConfigurationManager>();
            Assert.IsNotNull(candidate);
            Assert.IsInstanceOfType(candidate, typeof(AppConfigConfigurationManager));
        }
    }
}