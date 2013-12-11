namespace EnergyTrading.UnitTest.FileProcessing.Configuration
{
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.FileProcessing.Configuration;

    [TestClass]
    public class FileProcessorHostSectionFixture
    {
        [TestMethod]
        public void CanLoadEmptySection()
        {
            var section = ConfigurationManager.GetSection("fphEmpty") as FileProcessorHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(0, section.Processors.Count);
        }

        [TestMethod]
        public void CanLoadProcessorWithEmptyName()
        {
            var section = ConfigurationManager.GetSection("fphEmptyName") as FileProcessorHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(1, section.Processors.Count);
            Assert.IsTrue(string.IsNullOrEmpty(section.Processors[0].Name));
        }

        [TestMethod]
        public void CanLoadMultipleProcessors()
        {
            var section = ConfigurationManager.GetSection("fphMulti") as FileProcessorHostSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(2, section.Processors.Count);
        }
    }
}
