namespace EnergyTrading.UnitTest.Configuration
{
    using System.Configuration;

    using EnergyTrading.Configuration;
    using EnergyTrading.Test;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WcfServiceConfigurationSectionFixture : Fixture
    {
        [TestMethod]
        public void Load()
        {
            var section = (WcfServiceConfigurationSection)ConfigurationManager.GetSection("wcfServices");

            Assert.AreEqual(true, section.IsConsoleMode);
            Assert.AreEqual(3, section.Services.Count);

            this.Check2(section.Services[0], "product");
        }

        private void Check2(WcfServiceConfigurationElement candidate, string name)
        {
            Assert.AreEqual(name, candidate.Name, "Name differs");
        }
    }
}