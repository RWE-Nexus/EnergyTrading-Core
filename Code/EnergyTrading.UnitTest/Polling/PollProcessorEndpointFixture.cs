namespace EnergyTrading.UnitTest.Polling
{
    using System;

    using EnergyTrading.Polling;

    using NUnit.Framework;

    [TestFixture]
    public class PollProcessorEndpointFixture
    {
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValidateNullName()
        {
            new PollProcessorEndpoint().Validate();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValidateEmptyName()
        {
            new PollProcessorEndpoint { Name = string.Empty }.Validate();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValidateZeroInterval()
        {
            new PollProcessorEndpoint { Name = "test", IntervalSecs = 0 }.Validate();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValidateNegativeInterval()
        {
            new PollProcessorEndpoint { Name = "test", IntervalSecs = -1 }.Validate();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValidateInvalidHandler()
        {
            new PollProcessorEndpoint { Name = "test", IntervalSecs = -1 }.Validate();
        }

        [Test]
        public void ValidateValidEndpoint()
        {
            new PollProcessorEndpoint { Name = "test", IntervalSecs = 1, Handler = typeof(PollerImpl), Workers = 1 }.Validate();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValidateNegativeWorkers()
        {
            new PollProcessorEndpoint() {Name = "test", IntervalSecs = 1, Handler = typeof (PollerImpl), Workers = -1}
                .Validate();
        }
    }
}
