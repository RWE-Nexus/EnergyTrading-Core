namespace TradeEnrichment.BusAdapter.UnitTests.MessageFiltering.ByTrader
{
    using System.Collections.Generic;
    using System.Linq;

    using EnergyTrading.Configuration;
    using EnergyTrading.Filtering;

    using NUnit.Framework;

    [TestFixture]
    public class FilterExtensionsFixture
    {
        [Test]
        [TestCase("emptyFilterSection", "", "")]
        [TestCase("emptyFilterCollections", "", "")]
        [TestCase("emptyFilterElements", "", "")]
        [TestCase("onlyIncludeFilterElements", "inc", "")]
        [TestCase("onlyExcludeFilterElements", "", "exc")]
        [TestCase("allValidFilterElements", "inc1,inc2,inc3", "exc1,exc2,exc3")]
        public void ToFilterRepositoryFromConfigurationManager(string sectionName, string expectedIncludes, string expectedExcludes)
        {
            var includes = string.IsNullOrEmpty(expectedIncludes) ? new List<string>() : expectedIncludes.Split(',').ToList();
            var excludes = string.IsNullOrEmpty(expectedExcludes) ? new List<string>() : expectedExcludes.Split(',').ToList();
            var repository = new AppConfigConfigurationManager().ToFilterRepository(sectionName);
            Assert.AreEqual(repository.Included.Count, includes.Count);
            foreach (var item in includes)
            {
                Assert.IsTrue(repository.Included.Contains(item));
            }
            Assert.AreEqual(repository.Excluded.Count, excludes.Count);
            foreach (var item in excludes)
            {
                Assert.IsTrue(repository.Excluded.Contains(item));
            }
        }

        [Test]
        [TestCase("emptyFilterSection", "anything", false)]
        [TestCase("emptyFilterCollections", "anything", false)]
        [TestCase("emptyFilterElements", "anything", false)]
        [TestCase("onlyIncludeFilterElements", "inc", false)]
        [TestCase("onlyExcludeFilterElements", "exc", true)]
        [TestCase("allValidFilterElements", "inc1", false)]
        [TestCase("allValidFilterElements", "exc3", true)]
        public void ToFilterFromConfigManager(string sectionName, string inputValue, bool expectedResult)
        {
            var filter = new AppConfigConfigurationManager().ToFilter(sectionName);
            var candidate = filter.IsExcluded(inputValue);
            Assert.That(candidate, Is.EqualTo(expectedResult));
        }
    }
}