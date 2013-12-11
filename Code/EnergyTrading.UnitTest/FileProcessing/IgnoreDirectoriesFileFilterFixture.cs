namespace EnergyTrading.UnitTest.FileProcessing
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Configuration;
    using EnergyTrading.FileProcessing;

    [TestClass]
    public class IgnoreDirectoriesFileFilterFixture
    {
        private const string TestUncPath = @"\\server\share\dev\filedrop\test\file.txt";
        private const string TestLocalPath = @"C:\dev\filedrop\test\file.txt";

        private Mock<IConfigurationManager> mockConfig;

        [TestInitialize]
        public void SetUp()
        {
            mockConfig = new Mock<IConfigurationManager>();
        }

        private class IgnoreDirectoriesFileFilterHelper : IgnoreDirectoriesFileFilter
        {
            public IgnoreDirectoriesFileFilterHelper(IConfigurationManager config)
                : base(config)
            {
            }

            public IEnumerable<string> CallGetNonRootDirectoriesInFilePath(string fullPath)
            {
                return this.GetNonRootDirectoriesInFilePath(fullPath);
            }
        }

        [TestMethod]
        public void GetDirectoriesWithRootedPath()
        {
            mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", string.Empty }, { "IgnoreDirectoriesFilterMatchCase", "true" } });
            var candidate = new IgnoreDirectoriesFileFilterHelper(mockConfig.Object).CallGetNonRootDirectoriesInFilePath(TestLocalPath).ToList();
            Assert.AreEqual(candidate.Count, 3);
            Assert.AreEqual(candidate[0], "test");
            Assert.AreEqual(candidate[1], "filedrop");
            Assert.AreEqual(candidate[2], "dev");
        }

        [TestMethod]
        public void GetDirectoriesWithUncPath()
        {
            mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", string.Empty }, { "IgnoreDirectoriesFilterMatchCase", "true" } });
            var candidate = new IgnoreDirectoriesFileFilterHelper(mockConfig.Object).CallGetNonRootDirectoriesInFilePath(TestUncPath).ToList();
            Assert.AreEqual(candidate.Count, 3);
            Assert.AreEqual(candidate[0], "test");
            Assert.AreEqual(candidate[1], "filedrop");
            Assert.AreEqual(candidate[2], "dev");
        }

        private void TestIncludeFile(IEnumerable<string> directories, bool matchCase, bool includeFile)
        {
            var message = string.Empty;
            if (directories.Any())
            {
                message = directories.Aggregate((a, b) => a + "," + b);
            }
            mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", message }, { "IgnoreDirectoriesFilterMatchCase", matchCase ? "true" : "false" } });
            var candidate = new IgnoreDirectoriesFileFilter(mockConfig.Object).IncludeFile(TestLocalPath);
            Assert.AreEqual(candidate, includeFile, "localPath :" + message);
            candidate = new IgnoreDirectoriesFileFilter(mockConfig.Object).IncludeFile(TestUncPath);
            Assert.AreEqual(candidate, includeFile, "Unc Path :" + message);
        }

        [TestMethod]
        public void AllIncludedIfNoConfigurationValuesAreSet()
        {
            mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection());
            var candidate = new IgnoreDirectoriesFileFilter(mockConfig.Object).IncludeFile(TestLocalPath);
            Assert.IsTrue(candidate);
            candidate = new IgnoreDirectoriesFileFilter(mockConfig.Object).IncludeFile(TestUncPath);
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void RunIncludeFileTestCases()
        {
            const bool ShouldIncludeFile = true;
            const bool ShouldFilterFile = false;
            const bool CaseSensitive = true;
            const bool CaseInsensitive = false;

            TestIncludeFile(new List<string>(), CaseInsensitive, ShouldIncludeFile);
            TestIncludeFile(new List<string>(), CaseSensitive, ShouldIncludeFile);
            TestIncludeFile(new List<string> { "none", "of", "these", "exist" }, CaseInsensitive, ShouldIncludeFile);
            TestIncludeFile(new List<string> { "none", "of", "these", "exist" }, CaseSensitive, ShouldIncludeFile);
            TestIncludeFile(new List<string> { "Dev", "one", "of", "these", "is", "wrong", "case" }, CaseSensitive, ShouldIncludeFile);
            TestIncludeFile(new List<string> { "Dev", "one", "of", "these", "is", "wrong", "case" }, CaseInsensitive, ShouldFilterFile);
            TestIncludeFile(new List<string> { "filedrop", "one", "of", "these", "is", "correct", "case" }, CaseInsensitive, ShouldFilterFile);
            TestIncludeFile(new List<string> { "filedrop", "one", "of", "these", "is", "correct", "case" }, CaseSensitive, ShouldFilterFile);
            TestIncludeFile(new List<string> { "Dev", "filedrop", "multiple", "with", "incorrect", "case" }, CaseInsensitive, ShouldFilterFile);
            TestIncludeFile(new List<string> { "dev", "filedrop", "multiple", "with", "correct", "case" }, CaseSensitive, ShouldFilterFile);
            TestIncludeFile(new List<string> { "Dev", "Filedrop", "multiple", "all", "incorrect", "case" }, CaseInsensitive, ShouldFilterFile);
            TestIncludeFile(new List<string> { "Dev", "Filedrop", "multiple", "all", "incorrect", "case" }, CaseSensitive, ShouldIncludeFile);
        }

        [TestMethod]
        public void DefaultIsCaseInsensitive()
        {
            mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", "Dev,Filedrop,multiple,all,incorrect,case" } });
            var candidate = new IgnoreDirectoriesFileFilter(mockConfig.Object).IncludeFile(TestLocalPath);
            Assert.IsFalse(candidate);
        }
    }
}