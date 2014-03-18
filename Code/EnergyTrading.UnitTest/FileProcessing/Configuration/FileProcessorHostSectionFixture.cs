namespace EnergyTrading.UnitTest.FileProcessing.Configuration
{
    using System.Configuration;

    using EnergyTrading.FileProcessing.Configuration;

    using NUnit.Framework;

    [TestFixture]
    public class FileProcessorHostSectionFixture
    {
        [Test]
        public void CanLoadEmptySection()
        {
            var section = ConfigurationManager.GetSection("fphEmpty") as FileProcessorHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(0, section.Processors.Count);
        }

        [Test]
        public void CanLoadProcessorWithEmptyName()
        {
            var section = ConfigurationManager.GetSection("fphEmptyName") as FileProcessorHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(1, section.Processors.Count);
            Assert.IsTrue(string.IsNullOrEmpty(section.Processors[0].Name));
        }

        [Test]
        public void CanLoadMultipleProcessors()
        {
            var section = ConfigurationManager.GetSection("fphMulti") as FileProcessorHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(2, section.Processors.Count);
        }
    }
}
