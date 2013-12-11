namespace EnergyTrading.UnitTest.FileProcessing.Verification
{
    using System.Collections.Specialized;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Configuration;
    using EnergyTrading.FileProcessing.Verification;

    [TestClass]
    public class FileInfoExtensionsFixture
    {
        private readonly string prefix = "testPrefix";
        private Mock<IConfigurationManager> mockConfigManager;

        [TestInitialize]
        public void TestInitialize()
        {
            mockConfigManager = new Mock<IConfigurationManager>();
            mockConfigManager.Setup(x => x.AppSettings).Returns(new NameValueCollection { { ConfigurationManagerExtensions.VerificationPrefixAppSetting, prefix }});
        }

        [TestMethod]
        public void IsTestFileReturnsFalseIfFileInfoIsNull()
        {
            var candidate = FileInfoExtensions.IsTestFile(null, mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void IsTestFileReturnsFalseIfFileNameDoesNotStartWithPrefix()
        {
            var candidate = new FileInfo("someOtherFileName.txt").IsTestFile(mockConfigManager.Object);
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void IsTestFileReturnsTrueIfFileNameStartsWithPrefix()
        {
            var candidate = new FileInfo(prefix + "someOtherFileName.txt").IsTestFile(mockConfigManager.Object);
            Assert.IsTrue(candidate);
        }
    }
}