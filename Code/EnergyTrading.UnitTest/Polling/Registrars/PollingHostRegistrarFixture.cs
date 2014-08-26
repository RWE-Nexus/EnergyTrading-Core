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
            new PollingHostRegistrar { SectionName = "pollingHostEmpty" }.Register(container);
            container.Resolve<IPollingHost>();
        }

        [Test]
        public void WhenMultiWorkersAreConfiguredThenThereShouldBeMultipleEndPoints()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "phMultiWorkers" }.Register(container);
            var x = container.Resolve<PollProcessorEndpoint[]>();
            Assert.AreEqual(2, x.Length);
        }

        [Test]
        public void WhenMultiWorkersAreConfiguredThenEndPointsNamesShouldHaveNumericalSuffixes()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "phMultiWorkers" }.Register(container);
            Assert.AreEqual("Test_1", container.Resolve<PollProcessorEndpoint>("Test_1").Name);
            Assert.AreEqual("Test", container.Resolve<PollProcessorEndpoint>("Test").Name);
        }

        [Test]
        public void WhenMultiWorkersAreConfiguredWithSinglePollingThenThereShouldBeSingleEndPoint()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "phMultiWorkersSinglePolling" }.Register(container);
            var endpoints = container.Resolve<PollProcessorEndpoint[]>();
            Assert.AreEqual(1, endpoints.Length);
        }

        [Test]
        public void WhenMultiWorkersAreConfiguredWithSinglePollingThenWorkersCountShouldBeOne()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "phMultiWorkersSinglePolling" }.Register(container);
            var endpoint = container.Resolve<PollProcessorEndpoint[]>()[0];
            Assert.AreEqual(1, endpoint.Workers);
        }
    }
}
