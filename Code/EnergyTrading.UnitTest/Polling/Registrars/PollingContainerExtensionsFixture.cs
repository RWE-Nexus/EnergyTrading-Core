namespace EnergyTrading.UnitTest.Polling.Registrars
{
    using System;

    using EnergyTrading.Polling;
    using EnergyTrading.Polling.Registrars;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    [TestFixture]
    public class PollingContainerExtensionsFixture
    {
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void RegisterPollProcessorWithInvalidEndpoint()
        {
            var container = new UnityContainer();
            container.RegisterPollProcessor(new PollProcessorEndpoint());
        }

        [Test]
        public void RegisterPollProcessorWithValidEndpoint()
        {
            var container = new UnityContainer();
            container.RegisterPollProcessor(
                new PollProcessorEndpoint { Handler = typeof(PollerImpl), IntervalSecs = 1, Name = "Test Poller", Workers = 1, SinglePolling = true });
            container.Resolve<IPollProcessor>("Test Poller");
            container.Resolve<PollProcessorEndpoint>("Test Poller");
            container.Resolve<IPoller>("Test Poller");
        }
    }
}
