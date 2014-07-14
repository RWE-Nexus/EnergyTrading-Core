namespace EnergyTrading.Data.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;

    using EnergyTrading.Logging;

    /// <summary>
    /// Provides a <see cref="DbContext" />
    /// </summary>
    public class DbContextProvider : IDbContextProvider
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            syncLock = new object();
        }

        /// <copydocfrom cref="IDbContextProvider.CurrentContext" />
        public DbContext CurrentContext()
        {
            lock (syncLock)
            {
                if (logger.IsDebugEnabled)
                {
                    Debug.Write("Called");
                }
                return context ?? (context = func());
            }
        }

        /// <copydocfrom cref="IDbContextProvider.Close" />
        public void Close()
        {
            lock (syncLock)
            {
                if (logger.IsDebugEnabled)
                {
                    Debug.Write("Called");
                }
                if (context != null)
                {
                    if (logger.IsDebugEnabled)
                    {
                        Debug.Write("Disposed");
                    }
                    context.Dispose();
                }
                context = null;
            }
        }
    }
}