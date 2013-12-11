namespace EnergyTrading.Data
{
    using System;
    using System.Linq;

    /// <summary>
    /// Provides persistence methods for entities.
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    [Obsolete("Use IRepository")]
    public interface IRepository<T>
        where T : IIdentifiable
    {
        /// <summary>
        /// Find an entity based on its primary key.
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        T FindOne(object id);

        /// <summary>
        /// Get the entities.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Queryable();

        /// <summary>
        /// Add an entity.
        /// </summary>
        /// <param name="entity">Entity to use</param>
        void Add(T entity);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">Entity to use</param>
        void Delete(T entity);

        /// <summary>
        /// Evict an entity from the current repository, does not affect the database, just the in-memory copy.
        /// </summary>
        /// <param name="entity">Entity to use</param>
        void Evict(T entity);

        /// <summary>
        /// Save an entity.
        /// </summary>
        /// <param name="entity">Entity to use</param>
        void Save(T entity);

        /// <summary>
        /// Pushes changes to the database
        /// </summary>
        void Flush();
    }
}