namespace EnergyTrading.Data.EntityFramework
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.EntityClient;
    using System.Data.Objects;
    using System.Linq;

    using EnergyTrading.Data;

    [Obsolete("Use DbSetRepository")]
    public class DbSetRepository<T> : IRepository<T>, IDao
        where T : class, IIdentifiable
    {
        private readonly DbContext context;
        private readonly ObjectContext objectContext;
        private readonly DbSet<T> set;
        private readonly IDao dao;

        public DbSetRepository(DbContext context)
        {
            this.context = context;
            this.objectContext = (this.context as IObjectContextAdapter).ObjectContext;
            this.set = context.Set<T>();

            var entityConn = (EntityConnection)this.objectContext.Connection;
            var conn = entityConn.StoreConnection;
            this.dao = new Dao(conn);
        }

        IDbConnection IDao.Connection
        {
            get { return this.Connection; }
        }

        protected IDbConnection Connection
        {
            get { return this.dao.Connection; }
        }

        public void Add(T entity)
        {
            this.set.Add(entity);
        }

        public void Delete(T entity)
        {
            this.set.Remove(entity);
        }

        public void Evict(T entity)
        {
            this.ObjectSet().Detach(entity);
        }

        public T FindOne(object id)
        {
            return this.set.FindEx(id);
        }

        public IQueryable<T> Queryable() 
        {
            return this.set;
        }

        public void Save(T entity)
        {
            this.set.Add(entity);
        }

        public void Flush()
        {
            this.context.SaveChanges();
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
            return this.dao.ExecuteNonQuery(sql, timeout);
        }

        private IObjectSet<T> ObjectSet()
        {
            return this.objectContext.CreateObjectSet<T>();
        }
    }
}