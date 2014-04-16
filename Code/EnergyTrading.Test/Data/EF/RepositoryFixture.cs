namespace EnergyTrading.Test.Data.EF
{
    using System.Linq;
    using System.Transactions;

    using EnergyTrading.Data;

    using NUnit.Framework;

    public abstract class RepositoryFixture<T> : Fixture
        where T : class, IIdentifiable, new()
    {
        protected abstract IRepository Repository { get; set; }

        [Test]
        public void Save()
        {
            var expected = Default();

            using (var scope = new TransactionScope())
            {
                Repository.Add(expected);
                Repository.Flush();

                scope.Complete();
            }

            // NB Evict wipes collections - so won't compare correctly
            //this.Repository.Evict(expected);

            var count = (from x in Repository.Queryable<T>() select x).Count();
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RepositoryQuery()
        {
            var entities = from x in Repository.Queryable<T>() select x;

            var count = entities.Count();
        }

        protected override void OnSetup()
        {
            Zap();
        }

        protected override void OnTearDown()
        {
            Zap();
        }

        protected override void TidyUp()
        {
            base.TidyUp();
            Zap();
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