namespace EnergyTrading.Test.Data.EF
{
    using System.Data.Entity;

    using EnergyTrading.Data;

    using EnergyTrading.Data.EntityFramework;

    public abstract class DbSetGenericRepositoryFixture<T> : GenericRepositoryFixture<T>
        where T : class, IIdentifiable, new()
    {
        private IDbContextProvider contextProvider;
        private IRepository genericRepository;

        protected IDbContextProvider ContextProvider
        {
            get { return this.contextProvider ?? (this.contextProvider = new DbContextProvider(this.CreateDbContext)); }
        }

        protected DbContext Context
        {
            get { return this.ContextProvider.CurrentContext(); }
            //set { this.context = value; }
        }

        protected IRepository GenericRepository
        {
            get { return this.genericRepository ?? (this.genericRepository = new DbSetRepository(this.ContextProvider)); }
            //set { this.genericRepository = value; }
        }

        protected abstract DbContext CreateDbContext();
    }
}