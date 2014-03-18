namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Configuration;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Registrars;

    [TestFixture]
    public class ConfigurationManagerRegistrarFixture
    {
        [Test]
        public void CanResolveIConfigurationManager()
        {
            var container = new UnityContainer();
            new ConfigurationManagerRegistrar().Register(container);
            var candidate = container.Resolve<IConfigurationManager>();
            Assert.IsNotNull(candidate);
            Assert.IsInstanceOf<AppConfigConfigurationManager>(candidate);
        }
    }
}