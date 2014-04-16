namespace EnergyTrading.Test.Data.EF
{
    using System.Data.Entity;

    using EnergyTrading.Data;

    using EnergyTrading.Data.EntityFramework;

    public abstract class DbSetRepositoryFixture<T> : RepositoryFixture<T>
        where T : class, IIdentifiable, new()
    {
        private IDbContextProvider contextProvider;
        private IRepository repository;

        protected IDbContextProvider ContextProvider
        {
            get { return contextProvider ?? (contextProvider = new DbContextProvider(CreateDbContext)); }
        }

        protected DbContext Context
        {
            get { return ContextProvider.CurrentContext(); }
        }

        protected override IRepository Repository
        {
            get { return repository ?? (repository = new DbSetRepository(ContextProvider)); }
            set { repository = value; }
        }

        protected abstract DbContext CreateDbContext();
    }
}