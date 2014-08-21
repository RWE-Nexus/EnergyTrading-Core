namespace EnergyTrading.UnitTest.Polling
{
    using System;

    using EnergyTrading.Polling;
    using EnergyTrading.Test;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class PollingHostFixture
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNull()
        {
            new PollingHost(null);
        }

        [Test]
        public void StartCallsStartOnEachProcessor()
        {
            var mockPro1 = new Mock<IPollProcessor>();
            var mockPro2 = new Mock<IPollProcessor>();
            var counter1 = AssemblyLoggerProvider.MockLogger.StartCounting(x => x.Info("Starting"));
            var counter2 = AssemblyLoggerProvider.MockLogger.StartCounting(x => x.Info("Started"));
            var host = new PollingHost(new[] { mockPro1.Object, mockPro2.Object });
            host.Start();
            mockPro1.Verify(x => x.Start(), Times.Once());
            mockPro2.Verify(x => x.Start(), Times.Once());
            Assert.AreEqual(1, counter1.Count);
            Assert.AreEqual(1, counter2.Count);
        }

        [Test]
        public void BehaviourWhenProcessorThrowsOnStart()
        {
            var exCount = 0;
            var mockPro1 = new Mock<IPollProcessor>();
            mockPro1.Setup(x => x.Start()).Throws(new InvalidOperationException());
            var mockPro2 = new Mock<IPollProcessor>();
            try
            {
                var host = new PollingHost(new[] { mockPro1.Object, mockPro2.Object });
                host.Start();
            }
            catch (InvalidOperationException)
            {
                ++exCount;
            }
            mockPro1.Verify(x => x.Start(), Times.Once());
            mockPro2.Verify(x => x.Start(), Times.Never());
            mockPro2.Verify(x => x.Stop(), Times.AtLeast(2));
            mockPro1.Verify(x => x.Stop(), Times.AtLeast(2));
            AssemblyLoggerProvider.MockLogger.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.AtLeastOnce());
            Assert.AreEqual(1, exCount);
        }

        [Test]
        public void StopCallsStopOnEachProcessor()
        {
            var mockPro1 = new Mock<IPollProcessor>();
            var mockPro2 = new Mock<IPollProcessor>();
            var host = new PollingHost(new[] { mockPro1.Object, mockPro2.Object });
            var counter1 = AssemblyLoggerProvider.MockLogger.StartCounting(x => x.Info("Stopping"));
            var counter2 = AssemblyLoggerProvider.MockLogger.StartCounting(x => x.Info("Stopped"));
            host.Stop();
            mockPro1.Verify(x => x.Stop(), Times.Once());
            mockPro2.Verify(x => x.Stop(), Times.Once());
            Assert.AreEqual(1, counter1.Count);
            Assert.AreEqual(1, counter2.Count);
        }
    }
}
