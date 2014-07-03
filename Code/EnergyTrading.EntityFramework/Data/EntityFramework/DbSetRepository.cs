namespace EnergyTrading.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using EnergyTrading.Data;
    using EnergyTrading.Exceptions;

    public class DbSetRepository : IDbSetRepository, IRepository, IDao
    {
        private static readonly IExceptionFactory ExceptionFactory = new EntityFrameworkExceptionFactory();
        private readonly IDbContextProvider provider;
        private readonly Dictionary<Type, object> sets;
        private ObjectContext objectContext;

        public DbSetRepository(IDbContextProvider provider)
            : this(provider, new List<Action<IDbSetRepository>>())
        {
        }

        public DbSetRepository(IDbContextProvider provider, IList<Action<IDbSetRepository>> actions)
            : this(provider, actions, new List<Action<IDao>>())
        {
        }

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

        public IList<Action<IDbSetRepository>> Actions { get; set; }

        public IList<Action<IDao>> GlobalActions { get; set; }

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
                // NB Not sure about the lifetime of this if we've replaced the DbContext
                // if the error occurs
                if (objectContext != null)
                {
                    return objectContext;
                }
                objectContext = (Context as IObjectContextAdapter).ObjectContext;

                return objectContext;
            }
        }

        public void Add<T>(T entity)
            where T : class
        {
            DbSet<T>().Add(entity);
        }

        public void Delete<T>(T entity)
            where T : class
        {
            DbSet<T>().Remove(entity);
        }

        public void Attach<T>(T entity) where T : class
        {
            DbSet<T>().Attach(entity);
        }

        public void Evict<T>(T entity)
            where T : class
        {
            ObjectSet<T>().Detach(entity);
        }

        public T FindOne<T>(object id)
            where T : class
        {
            return DbSet<T>().FindEx(id);
        }

        public IQueryable<T> Queryable<T>()
            where T : class
        {
            return DbSet<T>();
        }

        public void Save<T>(T entity)
            where T : class
        {
            // NB EF does not have an explicit Save operation, all changes are automatically flushed by SaveChanges
        }

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
                    throw efex;
                }

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
            var entityConn = (EntityConnection)ObjectContext.Connection;
            var conn = entityConn.StoreConnection;
            return new Dao(conn);
        }

        private void Close()
        {
            sets.Clear();
            objectContext = null;
            provider.Close();
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

        private IObjectSet<T> ObjectSet<T>()
            where T : class
        {
            return ObjectContext.CreateObjectSet<T>();
        }
    }
}