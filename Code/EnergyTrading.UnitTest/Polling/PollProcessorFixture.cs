namespace EnergyTrading.UnitTest.Polling
{
    using System;
    using System.Threading;
    using EnergyTrading.Polling;
    using EnergyTrading.Polling.Registrars;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    [TestFixture]
    public class PollProcessorFixture
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullEndpoint()
        {
            new PollProcessor(null, new PollerImpl());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullPoller()
        {
            new PollProcessor(new PollProcessorEndpoint { Name = "name", IntervalSecs = 1, Handler = typeof(PollerImpl) }, null);
        }

        [Test]
        public void OnceStartedPollIsCalledOnTimer()
        {
            var poller = new PollerImpl();
            var processor = new PollProcessor(new PollProcessorEndpoint { Name = "name", IntervalSecs = 1, Handler = typeof(PollerImpl) }, poller);
            processor.Start();
            Thread.Sleep(3000);
            processor.Stop();
            Assert.GreaterOrEqual(poller.Count, 2);
        }

        [Test]
        public void TimerContinuesIfPollThrows()
        {
            var poller = new PollerImpl().ShouldThrow();
            var processor = new PollProcessor(new PollProcessorEndpoint { Name = "name", IntervalSecs = 1, Handler = typeof(PollerImpl) }, poller);
            processor.Start();
            Thread.Sleep(3000);
            processor.Stop();
            Assert.GreaterOrEqual(poller.Count, 2);
        }

        [Test]
        public void PollProcessorName()
        {
            var processor = new PollProcessor(new PollProcessorEndpoint { Name = "name", IntervalSecs = 1, Handler = typeof(PollerImpl) }, new PollerImpl());
            Assert.AreEqual("name", processor.Name);
        }

        [Test]
        public void WhenMultipleWorkersWithSinglePollingIsUsedPollerShouldBeCalledOnce()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "phMultiWorkersSinglePolling" }.Register(container);
            PollerImpl.PollCount = 0;
            var pollingHost = container.Resolve<PollingHost>();
            pollingHost.Start();
            Thread.Sleep(1500);
            pollingHost.Stop();
            Assert.AreEqual(1, PollerImpl.PollCount);
        }

        [Test]
        public void WhenMultipleWorkersAreUsedPollerShouldBeCalledMultipleTimes()
        {
            var container = new UnityContainer();
            new PollingHostRegistrar { SectionName = "phMultiWorkers" }.Register(container);
            PollerImpl.PollCount = 0;
            var pollingHost = container.Resolve<PollingHost>();
            pollingHost.Start();
            Thread.Sleep(1500);
            pollingHost.Stop();
            Assert.AreEqual(2, PollerImpl.PollCount);
        }

        [Test]
        public void WhenPollingInProgressThenPollingShouldNotBeCalledAgain()
        {
            var poller = new PollerImplWithDelay(1200);

            var processor = new PollProcessor(new PollProcessorEndpoint
            {
                Name = "Test",
                Handler = typeof(PollerImplWithDelay),
                IntervalSecs = 1,
                SinglePolling = true,
                Workers = 1
            }, poller);

            processor.Start();
            Thread.Sleep(2500);
            processor.Stop();

            Assert.AreEqual(1, poller.Count);
        }
    }
}
