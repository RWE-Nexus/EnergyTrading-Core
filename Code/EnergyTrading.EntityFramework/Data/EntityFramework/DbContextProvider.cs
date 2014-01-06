namespace EnergyTrading.Data.EntityFramework
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// Provides a <see cref="DbContext" />
    /// </summary>
    public class DbContextProvider : IDbContextProvider
    {
        private readonly Func<DbContext> func;
        private readonly object syncLock;
        private DbContext context;

        /// <summary>
        /// Creates a new instance of the <see cref="DbContextProvider" /> class.
        /// </summary>
        /// <param name="func"></param>
        public DbContextProvider(Func<DbContext> func)
        {
            this.func = func;
            this.syncLock = new object();
        }

        /// <copydocfrom cref="IDbContextProvider.CurrentContext" />
        public DbContext CurrentContext()
        {
            lock (syncLock)
            {
                return context ?? (context = func());
            }
        }

        /// <copydocfrom cref="IDbContextProvider.Close" />
        public void Close()
        {
            lock (syncLock)
            {
                context = null;
            }
        }
    }
}