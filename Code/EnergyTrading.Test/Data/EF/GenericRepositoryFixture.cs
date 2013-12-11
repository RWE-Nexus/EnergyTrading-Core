namespace EnergyTrading.Test.Data.EF
{
    using System.Transactions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Data;

    [TestClass]
    public abstract class GenericRepositoryFixture<T> : Fixture
        where T : class, IIdentifiable, new()
    {
        private IRepository<T> repository;

        protected IRepository<T> Repository
        {
            get { return this.repository ?? (this.repository = this.CreateRepository()); }
            set { this.repository = value; }
        }

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

            //using (var scope = new TransactionScope())
            //{
            //    var candidate = this.Repository.FindOne(expected.Id);
            //    Check(expected, candidate);

            //    scope.Complete();
            //}
        }

        //[TestInitialize]
        //public void Setup()
        //{
        //    this.Zap();
        //}

        //[TestCleanup]
        //public void TearDown()
        //{            
        //    this.Zap();
        //}

        protected override void TidyUp()
        {
            base.TidyUp();
            this.Zap();
        }

        protected abstract IRepository<T> CreateRepository();

        protected virtual void Zap()
        {            
        }

        protected virtual T Default()
        {
            return new T();
        }
    }
}
