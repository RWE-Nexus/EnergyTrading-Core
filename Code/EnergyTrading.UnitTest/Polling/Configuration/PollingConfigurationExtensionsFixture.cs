namespace EnergyTrading.UnitTest.Polling.Configuration
{
    using System;

    using EnergyTrading.Polling.Configuration;

    using NUnit.Framework;

    [TestFixture]
    public class PollingConfigurationExtensionsFixture
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToEndpointWithInvalidType()
        {
            new PollProcessorElement { Handler = "somecrap, here", IntervalSecs = 1, Name = "name" }.ToEndpoint();
        }

        [Test]
        public void ToEndpointWithValidElement()
        {
            var endpoint = new PollProcessorElement { Handler = "EnergyTrading.UnitTest.Polling.PollerImpl, EnergyTrading.UnitTest", IntervalSecs = 1, Name = "name" }.ToEndpoint();
            Assert.AreEqual(1, endpoint.IntervalSecs);
            Assert.AreEqual("name", endpoint.Name);
            Assert.AreEqual(typeof(PollerImpl), endpoint.Handler);
        }
    }
}
