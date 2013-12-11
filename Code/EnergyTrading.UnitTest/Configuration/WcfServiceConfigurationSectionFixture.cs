namespace EnergyTrading.UnitTest.Configuration
{
    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Configuration;
    using EnergyTrading.Test;

    [TestClass]
    public class WcfServiceConfigurationSectionFixture : Fixture
    {
        [TestMethod]
        public void Load()
        {
            var section = (WcfServiceConfigurationSection)ConfigurationManager.GetSection("wcfServices");

            Assert.AreEqual(true, section.IsConsoleMode);
            Assert.AreEqual(3, section.Services.Count);

            Check2(section.Services[0], "product");
        }

        private void Check2(WcfServiceConfigurationElement candidate, string name)
        {
            Assert.AreEqual(name, candidate.Name, "Name differs");
        }
    }
}