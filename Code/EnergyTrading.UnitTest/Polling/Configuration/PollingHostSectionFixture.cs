namespace EnergyTrading.UnitTest.Polling.Configuration
{
    using System.Configuration;

    using EnergyTrading.Polling.Configuration;

    using NUnit.Framework;

    [TestFixture]
    public class PollingHostSectionFixture
    {
        [Test]
        public void CanLoadEmptySection()
        {
            var section = ConfigurationManager.GetSection("pollingHostEmpty") as PollingHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(0, section.PollProcessors.Count);
        }

        [Test]
        public void CanLoadProcessorWithEmptyName()
        {
            var section = ConfigurationManager.GetSection("phEmptyName") as PollingHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(1, section.PollProcessors.Count);
            Assert.IsTrue(string.IsNullOrEmpty(section.PollProcessors[0].Name));
        }

        [Test]
        public void CanLoadMultipleProcessors()
        {
            var section = ConfigurationManager.GetSection("phMulti") as PollingHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(2, section.PollProcessors.Count);
        }
    }
}
