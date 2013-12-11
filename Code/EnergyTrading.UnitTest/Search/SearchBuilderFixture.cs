namespace EnergyTrading.UnitTest.Search
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Contracts.Search;
    using EnergyTrading.Search;

    [TestClass]
    public class SearchBuilderFixture : Fixture
    {
        [TestMethod]
        public void CreateAndSearch()
        {
            var expected = new Search
            {
                SearchFields = { Combinator = SearchCombinator.And },
                SearchOptions = { IsMappingSearch = false }, 
            };

            var candidate = SearchBuilder.CreateSearch();

            Check(expected, candidate);
        }

        [TestMethod]
        public void CreateOrMappingSearch()
        {
            var expected = new Search
            {
                SearchFields = { Combinator = SearchCombinator.Or },
                SearchOptions = { IsMappingSearch = true },
            };

            var candidate = SearchBuilder.CreateSearch(SearchCombinator.Or, true);

            Check(expected, candidate);
        }

        [TestMethod]
        public void AssignPageSize()
        {
            var search = new Search();
            search.MaxPageSize(10);

            Assert.AreEqual(10, search.SearchOptions.ResultsPerPage, "ResultsPerPage differs");
        }

        [TestMethod]
        public void AssignNoPageSize()
        {
            var search = new Search();
            search.NoMaxPageSize();

            Assert.IsNull(search.SearchOptions.ResultsPerPage, "ResultsPerPage differs");
        }

        [TestMethod]
        public void AssignMultiPage()
        {
            var search = new Search();
            search.IsMutliPage();

            Assert.AreEqual(true, search.SearchOptions.MultiPage, "MultiPage differs");
        }

        [TestMethod]
        public void AssignNotMultiPage()
        {
            var search = new Search();
            search.NotMultiPage();

            Assert.AreEqual(false, search.SearchOptions.MultiPage, "MultiPage differs");
        }
    }
}