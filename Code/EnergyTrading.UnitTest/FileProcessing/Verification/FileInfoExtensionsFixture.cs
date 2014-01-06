namespace EnergyTrading.UnitTest.FileProcessing.Verification
{
    using System.Collections.Specialized;
    using System.IO;

    using EnergyTrading.Configuration;
    using EnergyTrading.FileProcessing.Verification;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class FileInfoExtensionsFixture
    {
        private readonly string prefix = "testPrefix";
        private Mock<IConfigurationManager> mockConfigManager;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockConfigManager = new Mock<IConfigurationManager>();
            this.mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationPrefixAppSetting, this.prefix }});
        }

        [TestMethod]
        public void IsTestFileReturnsFalseIfFileInfoIsNull()
        {
            var candidate = FileInfoExtensions.IsTestFile(null, this.mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void IsTestFileReturnsFalseIfFileNameDoesNotStartWithPrefix()
        {
            var candidate = new FileInfo("someOtherFileName.txt").IsTestFile(this.mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void IsTestFileReturnsTrueIfFileNameStartsWithPrefix()
        {
            var candidate = new FileInfo(this.prefix + "someOtherFileName.txt").IsTestFile(this.mockConfigManager.Object);
            Assert.IsTrue(candidate);
        }
    }
}