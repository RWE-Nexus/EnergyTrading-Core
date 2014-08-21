namespace EnergyTrading.UnitTest.Polling
{
    using System;
    using System.Threading;

    using EnergyTrading.Polling;

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
    }
}
