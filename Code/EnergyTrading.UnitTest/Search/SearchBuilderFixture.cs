namespace EnergyTrading.UnitTest.Search
{
    using EnergyTrading.Search;

    using EnergyTrading.Contracts.Search;

    using NUnit.Framework;

    [TestFixture]
    public class SearchBuilderFixture : Fixture
    {
        [Test]
        public void CreateAndSearch()
        {
            var expected = new Search
            {
                SearchFields = { Combinator = SearchCombinator.And },
                SearchOptions = { IsMappingSearch = false }, 
            };

            var candidate = SearchBuilder.CreateSearch();

            this.Check(expected, candidate);
        }

        [Test]
        public void CreateOrMappingSearch()
        {
            var expected = new Search
            {
                SearchFields = { Combinator = SearchCombinator.Or },
                SearchOptions = { IsMappingSearch = true },
            };

            var candidate = SearchBuilder.CreateSearch(SearchCombinator.Or, true);

            this.Check(expected, candidate);
        }

        [Test]
        public void AssignPageSize()
        {
            var search = new Search();
            search.MaxPageSize(10);

            Assert.AreEqual(10, search.SearchOptions.ResultsPerPage, "ResultsPerPage differs");
        }

        [Test]
        public void AssignNoPageSize()
        {
            var search = new Search();
            search.NoMaxPageSize();

            Assert.IsNull(search.SearchOptions.ResultsPerPage, "ResultsPerPage differs");
        }

        [Test]
        public void AssignMultiPage()
        {
            var search = new Search();
            search.IsMutliPage();

            Assert.AreEqual(true, search.SearchOptions.MultiPage, "MultiPage differs");
        }

        [Test]
        public void AssignNotMultiPage()
        {
            var search = new Search();
            search.NotMultiPage();

            Assert.AreEqual(false, search.SearchOptions.MultiPage, "MultiPage differs");
        }
    }
}