namespace EnergyTrading.UnitTest.Search
{
    using System.Web;

    using EnergyTrading.Contracts.Search;
    using EnergyTrading.Search;

    using NUnit.Framework;

    [TestFixture]
    public class SearchExtensionsFixture
    {
        [Test]
        public void KeysForSameSearchAreTheSame()
        {
            var search = SearchBuilder.CreateSearch();
            search.AddSearchCriteria(SearchCombinator.And)
                    .AddCriteria("TargetSourceSystem.Id", SearchCondition.Equals, "1", true)
                    .AddCriteria("SourceSystemType", SearchCondition.Equals, "SourceSystem", false);
            var key1 = search.ToKey<int>();
            var key2 = search.ToKey<int>();
            Assert.AreEqual(key1, key2);
        }

        [Test]
        public void SearchKeysAreReversible()
        {
            var search = SearchBuilder.CreateSearch();
            search.AddSearchCriteria(SearchCombinator.And)
                    .AddCriteria("TargetSourceSystem.Id", SearchCondition.Equals, "1", true)
                    .AddCriteria("SourceSystemType", SearchCondition.Equals, "SourceSystem", false);
            var key1 = search.ToKey<int>();
            var candidate = key1.ToSearch<int>();
            Assert.AreEqual(candidate.SearchFields.Combinator, SearchCombinator.And);
            Assert.AreEqual(candidate.SearchFields.Criterias[0].Criteria[0].Field, "TargetSourceSystem.Id");
            Assert.AreEqual(candidate.SearchFields.Criterias[0].Criteria[1].Field, "SourceSystemType");
            Assert.AreEqual(candidate.SearchFields.Criterias[0].Criteria[0].Condition, SearchCondition.Equals);
            Assert.AreEqual(candidate.SearchFields.Criterias[0].Criteria[1].Condition, SearchCondition.Equals);
            Assert.AreEqual(candidate.SearchFields.Criterias[0].Criteria[0].ComparisonValue, "1");
            Assert.AreEqual(candidate.SearchFields.Criterias[0].Criteria[1].ComparisonValue, "SourceSystem");
        }

        [Test]
        public void KeyIsNotAffectedByUrlEncoding()
        {
            var search = SearchBuilder.CreateSearch();
            search.AddSearchCriteria(SearchCombinator.And)
                    .AddCriteria("TargetSourceSystem.Id", SearchCondition.Equals, "1", true)
                    .AddCriteria("SourceSystemType", SearchCondition.Equals, "SourceSystem", false);
            var key1 = search.ToKey<int>();
            var urlEncodedKey = HttpUtility.UrlEncode(key1);
            Assert.AreEqual(key1, urlEncodedKey);
        }
    }
}