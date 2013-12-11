namespace EnergyTrading.Test.Data.EF
{
    using System.Linq;
    using System.Transactions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Data;

    [TestClass]
    [Ignore]
    public abstract class RepositoryFixture<T> : Fixture
        where T : class, IIdentifiable, new()
    {
        protected abstract IRepository Repository { get; set; }

        [TestMethod]
        public void Save()
        {
            var expected = this.Default();

            using (var scope = new TransactionScope())
            {
                this.Repository.Add(expected);
                this.Repository.Flush();

                scope.Complete();
            }

            // NB Evict wipes collections - so won't compare correctly
            //this.Repository.Evict(expected);

            var count = (from x in Repository.Queryable<T>() select x).Count();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void RepositoryQuery()
        {
            var entities = from x in this.Repository.Queryable<T>() select x;

            var count = entities.Count();
        }

        //[TestInitialize]
        protected override void OnSetup()
        {
            this.Zap();
        }

        //[TestCleanup]
        protected override void OnTearDown()
        {
            this.Zap();
        }

        protected override void TidyUp()
        {
            base.TidyUp();
            this.Zap();
        }

        protected virtual void Zap()
        {
        }

        protected virtual T Default()
        {
            return new T();
        }
    }
}