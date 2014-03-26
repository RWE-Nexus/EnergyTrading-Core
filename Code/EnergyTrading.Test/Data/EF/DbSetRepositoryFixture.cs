namespace EnergyTrading.Test.Data.EF
{
    using System.Data.Entity;

    using EnergyTrading.Data;

    using NUnit.Framework;

    using EnergyTrading.Data.EntityFramework;

    [TestFixture]
    public abstract class DbSetRepositoryFixture<T> : RepositoryFixture<T>
        where T : class, IIdentifiable, new()
    {
        private IDbContextProvider contextProvider;
        private IRepository repository;

        protected IDbContextProvider ContextProvider
        {
            get { return this.contextProvider ?? (this.contextProvider = new DbContextProvider(this.CreateDbContext)); }
        }

        protected DbContext Context
        {
            get { return this.ContextProvider.CurrentContext(); }
            //set { this.context = value; }
        }

        protected override IRepository Repository
        {
            get { return this.repository ?? (this.repository = new DbSetRepository(this.ContextProvider)); }
            set { this.repository = value; }
        }

        protected abstract DbContext CreateDbContext();
    }
}