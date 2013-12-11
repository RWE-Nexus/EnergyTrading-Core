namespace EnergyTrading.UnitTest.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Configuration;

    [TestClass]
    public class ConfigurationManagerExtensionsTests
    {
        private Mock<IConfigurationManager> mockConfigManager;

        [TestInitialize]
        public void TestInitialize()
        {
            mockConfigManager = new Mock<IConfigurationManager>();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGetConnectionSettingsWithPasswordThrowsIfNamedConnectionIsNotPresent()
        {
            mockConfigManager.Setup(x => x.ConnectionStrings).Returns(new ConnectionStringSettingsCollection());
            mockConfigManager.Object.GetConnectionSettingsWithPassword("testName");
        }

        [TestMethod]
        public void TestReturnedSettingsMatchOriginalSectionIfNoEncryptedPasswordIsPresent()
        {
            var settings = new ConnectionStringSettings("testName", "testconnectionString", "testProviderName");
            mockConfigManager.Setup(x => x.ConnectionStrings).Returns(new ConnectionStringSettingsCollection { settings });
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection());
            var result = mockConfigManager.Object.GetConnectionSettingsWithPassword("testName");
            Assert.AreEqual(settings.Name, result.Name);
            Assert.AreEqual(settings.ConnectionString, result.ConnectionString);
            Assert.AreEqual(settings.ProviderName, result.ProviderName);
        }

        [TestMethod]
        public void TestSettingsIncludesAdditionalInformationIfEncryptedPasswordIsFound()
        {
            var settings = new ConnectionStringSettings("testName", "testconnectionString", "testProviderName");
            mockConfigManager.Setup(x => x.ConnectionStrings).Returns(new ConnectionStringSettingsCollection { settings });
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "testName.EncryptedPassword", "mFVE3Qkch1nlozDtIkbjovrTFmcbm9e+YRO0WoCkpsA=" } });
            var result = mockConfigManager.Object.GetConnectionSettingsWithPassword("testName");
            Assert.AreEqual(settings.Name, result.Name);
            Assert.AreEqual(settings.ConnectionString + ";Password=testpassword", result.ConnectionString);
            Assert.AreEqual(settings.ProviderName, result.ProviderName);
        }

        [TestMethod]
        public void GetVerificationPrefixReturnsDefaultIfConfigurationManagerIsNull()
        {
            var candidate = ConfigurationManagerExtensions.GetVerificationPrefix(null);
            Assert.AreEqual(ConfigurationManagerExtensions.DefaultVerificationPrefix, candidate);
        }

        [TestMethod]
        public void GetVerificationPrefixReturnsDefaultIfNoAppSetting()
        {
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection());
            var candidate = this.mockConfigManager.Object.GetVerificationPrefix();
            Assert.AreEqual(ConfigurationManagerExtensions.DefaultVerificationPrefix, candidate);
        }

        [TestMethod]
        public void GetVerificationPrefixReturnsDefaultIfAppSettingIsWhiteSpace()
        {
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationPrefixAppSetting, "  " } });
            var candidate = this.mockConfigManager.Object.GetVerificationPrefix();
            Assert.AreEqual(ConfigurationManagerExtensions.DefaultVerificationPrefix, candidate);
        }

        [TestMethod]
        public void GetVerificationPrefixReturnsAppSettingIfSupplied()
        {
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationPrefixAppSetting, "newPrefix" } });
            var candidate = this.mockConfigManager.Object.GetVerificationPrefix();
            Assert.AreEqual("newPrefix", candidate);
        }

        [TestMethod]
        public void GetVerificationEnvironmentReturnsNullIfConfigurationManagerIsNull()
        {
            var candidate = ConfigurationManagerExtensions.GetVerificationEnvironment(null);
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void GetVerificationEnvironmentReturnsNullIfNoAppSetting()
        {
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection());
            var candidate = this.mockConfigManager.Object.GetVerificationEnvironment();
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void GetVerificationEnvironmentReturnsNullIfAppSettingIsWhiteSpace()
        {
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationEnvironmentPrefixAppSetting, "  " } });
            var candidate = this.mockConfigManager.Object.GetVerificationEnvironment();
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void GetVerificationEnvironmentReturnsAppSettingIfSupplied()
        {
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationEnvironmentPrefixAppSetting, "newPrefix" } });
            var candidate = this.mockConfigManager.Object.GetVerificationEnvironment();
            Assert.AreEqual("newPrefix", candidate);
        }
    }
}