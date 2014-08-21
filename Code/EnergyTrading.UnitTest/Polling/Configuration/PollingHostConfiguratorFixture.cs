namespace EnergyTrading.UnitTest.Polling.Configuration
{
    using EnergyTrading.Container.Unity;
    using EnergyTrading.Polling.Configuration;
    using EnergyTrading.Polling.Registrars;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    [TestFixture]
    public class PollingHostConfiguratorFixture
    {
        [Test]
        public void TestContainerPropertyIsNotNull()
        {
            var container = new PollingHostConfigurator().Container;
            Assert.IsNotNull(container);
        }

        [Test]
        public void TestContainerIsSetAsSupplied()
        {
            var container = new UnityContainer();
            var candidate = new PollingHostConfigurator(container).Container;
            Assert.AreSame(container, candidate);
        }

        [Test]
        public void TestStandardRegistrars()
        {
            var container = new UnityContainer();
            var configurator = new PollingHostConfigurator(container);
            configurator.StandardRegistrars();
            var candidate = UnityContainerExtensions.Resolve<IContainerRegistrar>(container, typeof(PollingHostRegistrar).FullName);
            Assert.AreEqual(typeof(PollingHostRegistrar), candidate.GetType());
        }
    }
}
