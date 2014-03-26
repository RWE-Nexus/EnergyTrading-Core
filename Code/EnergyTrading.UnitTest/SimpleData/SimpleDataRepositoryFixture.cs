namespace EnergyTrading.UnitTest.SimpleData
{
    using System.Collections.Generic;

    using EnergyTrading.Data.SimpleData;

    using NUnit.Framework;

    using Simple.Data;

    [TestFixture]
    public class SimpleDataRepositoryFixture
    {
        public class SimpleDataRepositoryHelper : SimpleDataRepository
        {
            public SimpleDataRepositoryHelper(Database database, string schema = "", int maxRetries = 3, SimpleDataMode mode = SimpleDataMode.Test)
                : base(database, schema, maxRetries, mode)
            {
            }

            public dynamic GetUsers()
            {
                return this.PerformDatabaseAction(() => this.GetSchemaObject().Users.All());
            }
        }

        [Test]
        public void CanUseSimpleDataMockingFunctionalityInTestMode()
        {
            var mockAdapter = new InMemoryAdapter();
            mockAdapter.Insert("Users", new Dictionary<string, object> { { "Id", 1 }, { "Name", "Albert" } }, false);
            Database.UseMockAdapter(mockAdapter);
            var helper = new SimpleDataRepositoryHelper(Database.Open());
            var rowsList = helper.GetUsers().ToList();
            Assert.AreEqual(1, rowsList.Count);
            Assert.AreEqual(1, rowsList[0].Id);
            Assert.AreEqual("Albert", rowsList[0].Name);
        }
    }
}