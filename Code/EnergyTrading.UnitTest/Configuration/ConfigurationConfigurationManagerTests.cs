namespace EnergyTrading.UnitTest.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using EnergyTrading.Configuration;

    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationConfigurationManagerTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCannotConstructWithNullConfig()
        {
            new ConfigurationConfigurationManager(null);
        }

        [Test]
        public void CanGetDefaultAppSettings()
        {
            var config = new ConfigurationConfigurationManager(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.empty.config") }, ConfigurationUserLevel.None));
            Assert.IsFalse(config.AppSettings.AllKeys.Contains("key"));
        }

        [Test]
        public void TestAppSettingsHasCorrectValues()
        {
            var config = new ConfigurationConfigurationManager(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.configmanagertest.config") }, ConfigurationUserLevel.None));
            Assert.AreEqual("value", config.AppSettings["key"]);
            Assert.AreEqual("another value", config.AppSettings["another key"]);
        }

        [Test]
        public void TestConnectionStringsHasCorrectValues()
        {
            var config = new ConfigurationConfigurationManager(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.configmanagertest.config") }, ConfigurationUserLevel.None));
            Assert.AreEqual("Data Source=.;Initial Catalog=EventMonitor;Integrated Security=yes", config.ConnectionStrings["EventMonitoring"].ConnectionString);
        }
    }
}
