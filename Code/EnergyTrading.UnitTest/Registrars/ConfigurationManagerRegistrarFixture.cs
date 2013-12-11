namespace EnergyTrading.UnitTest.Registrars
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Configuration;
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