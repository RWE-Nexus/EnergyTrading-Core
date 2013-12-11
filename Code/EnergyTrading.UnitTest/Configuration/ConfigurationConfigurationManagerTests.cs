namespace EnergyTrading.UnitTest.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Configuration;

    [TestClass]
    [DeploymentItem("App.configmanagertest.config")]
    [DeploymentItem("App.empty.config")]
    public class ConfigurationConfigurationManagerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCannotConstructWithNullConfig()
        {
            new ConfigurationConfigurationManager(null);
        }

        [TestMethod]
        public void CanGetDefaultAppSettings()
        {
            var config = new ConfigurationConfigurationManager(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.empty.config") }, ConfigurationUserLevel.None));
            Assert.IsFalse(config.AppSettings.AllKeys.Contains("key"));
        }

        [TestMethod]
        public void TestAppSettingsHasCorrectValues()
        {
            var config = new ConfigurationConfigurationManager(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.configmanagertest.config") }, ConfigurationUserLevel.None));
            Assert.AreEqual("value", config.AppSettings["key"]);
            Assert.AreEqual("another value", config.AppSettings["another key"]);
        }

        [TestMethod]
        public void TestConnectionStringsHasCorrectValues()
        {
            var config = new ConfigurationConfigurationManager(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.configmanagertest.config") }, ConfigurationUserLevel.None));
            Assert.AreEqual("Data Source=.;Initial Catalog=EventMonitor;Integrated Security=yes", config.ConnectionStrings["EventMonitoring"].ConnectionString);
        }
    }
}
