namespace EnergyTrading.UnitTest.FileProcessing
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using EnergyTrading.Configuration;
    using EnergyTrading.FileProcessing;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class IgnoreDirectoriesFileFilterFixture
    {
        private const string TestUncPath = @"\\server\share\dev\filedrop\test\file.txt";
        private const string TestLocalPath = @"C:\dev\filedrop\test\file.txt";

        private Mock<IConfigurationManager> mockConfig;

        [SetUp]
        public void SetUp()
        {
            this.mockConfig = new Mock<IConfigurationManager>();
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

        [Test]
        public void GetDirectoriesWithRootedPath()
        {
            this.mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", string.Empty }, { "IgnoreDirectoriesFilterMatchCase", "true" } });
            var candidate = new IgnoreDirectoriesFileFilterHelper(this.mockConfig.Object).CallGetNonRootDirectoriesInFilePath(TestLocalPath).ToList();
            Assert.AreEqual(candidate.Count, 3);
            Assert.AreEqual(candidate[0], "test");
            Assert.AreEqual(candidate[1], "filedrop");
            Assert.AreEqual(candidate[2], "dev");
        }

        [Test]
        public void GetDirectoriesWithUncPath()
        {
            this.mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", string.Empty }, { "IgnoreDirectoriesFilterMatchCase", "true" } });
            var candidate = new IgnoreDirectoriesFileFilterHelper(this.mockConfig.Object).CallGetNonRootDirectoriesInFilePath(TestUncPath).ToList();
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
            this.mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", message }, { "IgnoreDirectoriesFilterMatchCase", matchCase ? "true" : "false" } });
            var candidate = new IgnoreDirectoriesFileFilter(this.mockConfig.Object).IncludeFile(TestLocalPath);
            Assert.AreEqual(candidate, includeFile, "localPath :" + message);
            candidate = new IgnoreDirectoriesFileFilter(this.mockConfig.Object).IncludeFile(TestUncPath);
            Assert.AreEqual(candidate, includeFile, "Unc Path :" + message);
        }

        [Test]
        public void AllIncludedIfNoConfigurationValuesAreSet()
        {
            this.mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection());
            var candidate = new IgnoreDirectoriesFileFilter(this.mockConfig.Object).IncludeFile(TestLocalPath);
            Assert.IsTrue(candidate);
            candidate = new IgnoreDirectoriesFileFilter(this.mockConfig.Object).IncludeFile(TestUncPath);
            Assert.IsTrue(candidate);
        }

        [Test]
        public void RunIncludeFileTestCases()
        {
            const bool ShouldIncludeFile = true;
            const bool ShouldFilterFile = false;
            const bool CaseSensitive = true;
            const bool CaseInsensitive = false;

            this.TestIncludeFile(new List<string>(), CaseInsensitive, ShouldIncludeFile);
            this.TestIncludeFile(new List<string>(), CaseSensitive, ShouldIncludeFile);
            this.TestIncludeFile(new List<string> { "none", "of", "these", "exist" }, CaseInsensitive, ShouldIncludeFile);
            this.TestIncludeFile(new List<string> { "none", "of", "these", "exist" }, CaseSensitive, ShouldIncludeFile);
            this.TestIncludeFile(new List<string> { "Dev", "one", "of", "these", "is", "wrong", "case" }, CaseSensitive, ShouldIncludeFile);
            this.TestIncludeFile(new List<string> { "Dev", "one", "of", "these", "is", "wrong", "case" }, CaseInsensitive, ShouldFilterFile);
            this.TestIncludeFile(new List<string> { "filedrop", "one", "of", "these", "is", "correct", "case" }, CaseInsensitive, ShouldFilterFile);
            this.TestIncludeFile(new List<string> { "filedrop", "one", "of", "these", "is", "correct", "case" }, CaseSensitive, ShouldFilterFile);
            this.TestIncludeFile(new List<string> { "Dev", "filedrop", "multiple", "with", "incorrect", "case" }, CaseInsensitive, ShouldFilterFile);
            this.TestIncludeFile(new List<string> { "dev", "filedrop", "multiple", "with", "correct", "case" }, CaseSensitive, ShouldFilterFile);
            this.TestIncludeFile(new List<string> { "Dev", "Filedrop", "multiple", "all", "incorrect", "case" }, CaseInsensitive, ShouldFilterFile);
            this.TestIncludeFile(new List<string> { "Dev", "Filedrop", "multiple", "all", "incorrect", "case" }, CaseSensitive, ShouldIncludeFile);
        }

        [Test]
        public void DefaultIsCaseInsensitive()
        {
            this.mockConfig.Setup(x => x.AppSettings).Returns(new NameValueCollection { { "IgnoreDirectoriesFilterList", "Dev,Filedrop,multiple,all,incorrect,case" } });
            var candidate = new IgnoreDirectoriesFileFilter(this.mockConfig.Object).IncludeFile(TestLocalPath);
            Assert.IsFalse(candidate);
        }
    }
}