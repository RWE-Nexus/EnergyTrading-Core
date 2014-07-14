namespace EnergyTrading.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using EnergyTrading.Data;
    using EnergyTrading.Exceptions;
    using EnergyTrading.Logging;

    /// <summary>
    /// EF implementation of <see cref="IRepository"/>
    /// </summary>
    public class DbSetRepository : IDbSetRepository, IRepository, IDao
    {
        private static readonly ILogger logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IExceptionFactory ExceptionFactory = new EntityFrameworkExceptionFactory();
        private readonly IDbContextProvider provider;
        private readonly Dictionary<Type, object> sets;

        /// <summary>
        /// Creates a new instance of the <see cref="DbSetRepository"/> class.
        /// </summary>
        /// <param name="provider"></param>
        public DbSetRepository(IDbContextProvider provider)
            : this(provider, new List<Action<IDbSetRepository>>())
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DbSetRepository"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="actions"></param>
        public DbSetRepository(IDbContextProvider provider, IList<Action<IDbSetRepository>> actions)
            : this(provider, actions, new List<Action<IDao>>())
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DbSetRepository"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="actions"></param>
        /// <param name="globalActions"></param>
        public DbSetRepository(IDbContextProvider provider, IList<Action<IDbSetRepository>> actions, IList<Action<IDao>> globalActions)
        {
            this.provider = provider;
            sets = new Dictionary<Type, object>();
            Actions = actions;
            GlobalActions = globalActions;
        }

        IDbConnection IDao.Connection
        {
            get { return Connection; }
        }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        public IList<Action<IDbSetRepository>> Actions { get; set; }

        /// <summary>
        /// Gets or sets global actions.
        /// </summary>
        public IList<Action<IDao>> GlobalActions { get; set; }

        /// <summary>
        /// Gets the active connection.
        /// </summary>
        protected IDbConnection Connection
        {
            get { return Dao.Connection; }
        }

        private DbContext Context
        {
            get { return provider.CurrentContext(); }
        }

        private IDao Dao
        {
            get { return CreateDao(); }
        }

        private ObjectContext ObjectContext
        {
            get
            {
                // NB Don't cache this so we always get the current Context accessed.
                var objectContext = (Context as IObjectContextAdapter).ObjectContext;

                return objectContext;
            }
        }

        /// <copydoc cref="IRepository.Add{T}" />
        public void Add<T>(T entity)
            where T : class
        {
            try
            {
                DbSet<T>().Add(entity);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <copydoc cref="IRepository.Attach{T}" />
        public void Attach<T>(T entity) where T : class
        {
            try
            { 
                DbSet<T>().Attach(entity);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <copydoc cref="IRepository.Delete{T}" />
        public void Delete<T>(T entity)
            where T : class
        {
            try
            {
                DbSet<T>().Remove(entity);
            }           
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <copydoc cref="IRepository.Evict{T}" />
        public void Evict<T>(T entity)
            where T : class
        {
            try
            {
                ObjectContext.Detach(entity);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <copydoc cref="IRepository.FindOne{T}" />
        public T FindOne<T>(object id)
            where T : class
        {
            try
            {
                return DbSet<T>().FindEx(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <copydoc cref="IRepository.Queryable{T}" />
        public IQueryable<T> Queryable<T>()
            where T : class
        {
            try
            {
                return DbSet<T>();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        /// <copydoc cref="IRepository.Save{T}" />
        /// <remarks>EF does not have an explicit Save operation, all changes are persisted by <see cref="Flush"/></remarks>
        public void Save<T>(T entity)
            where T : class
        {
        }

        /// <copydoc cref="IRepository.Flush" />
        /// <remarks>We apply <see cref="GlobalActions"/> and <see cref="Actions"/> before calling <see cref="DbContext.SaveChanges"/></remarks>
        public void Flush()
        {
            try
            {
                foreach (var globalAction in GlobalActions)
                {
                    globalAction(this);
                }

                foreach (var action in Actions)
                {
                    action(this);
                }
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Close();
                var efex = ExceptionFactory.Convert(ex);
                if (efex != null)
                {
                    logger.Error(efex.Message);
                    throw efex;
                }

                logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes a non-query SQL command.
        /// <para>
        /// This is deliberately not on the public interface to discourage usage.
        /// </para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeout"></param>
        int IDao.ExecuteNonQuery(string sql, int timeout)
        {
            return Dao.ExecuteNonQuery(sql, timeout);
        }

        private IDao CreateDao()
        {
            var conn = Context.Database.Connection;
            return new Dao(conn);
        }

        private void Close()
        {
            try
            {
                sets.Clear();
                provider.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public IDbSet<T> DbSet<T>()
            where T : class
        {
            var type = typeof(T);
            object set;
            if (!sets.TryGetValue(type, out set))
            {
                sets[type] = set = Context.Set<T>();
            }

            return (IDbSet<T>)set;
        }
    }
}