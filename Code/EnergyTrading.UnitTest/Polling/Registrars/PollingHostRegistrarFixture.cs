namespace EnergyTrading.UnitTest.Polling.Registrars
{
    using EnergyTrading.Polling;
    using EnergyTrading.Polling.Registrars;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    [TestFixture]
    public class PollingHostRegistrarFixture
    {
        [Test]
        public void CanResolveIPollingHost()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "pollingHostEmpty"}.Register(container);
            container.Resolve<IPollingHost>();
        }
    }
}
