namespace EnergyTrading.Data
{
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// Provides persistence methods for entities.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Find an entity based on its primary key.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        T FindOne<T>(object id) where T : class;

        /// <summary>
        /// Get the entities.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <returns></returns>
        IQueryable<T> Queryable<T>() where T : class;

        /// <summary>
        /// Add an entity.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to use</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to use</param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Evict an entity from the current repository, does not affect the database, just the in-memory copy.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to use</param>
        void Evict<T>(T entity) where T : class;

        /// <summary>
        /// Save an entity.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to use</param>
        void Save<T>(T entity) where T : class;

        /// <summary>
        /// Flush changes to the database.
        /// <para>
        /// Performs validation, but will not actually commit if inside a <see cref="TransactionScope" />
        /// </para>
        /// </summary>
        void Flush();
    }
}