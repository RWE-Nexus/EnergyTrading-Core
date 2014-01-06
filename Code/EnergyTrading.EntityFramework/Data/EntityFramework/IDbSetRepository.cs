namespace EnergyTrading.Data.EntityFramework
{
    using System.Data.Entity;

    /// <summary>
    /// Expose the underlying <see cref="IDbSet{T}" /> for a repository.
    /// </summary>
    public interface IDbSetRepository
    {
        /// <summary>
        /// Gets the DbSet for an entity.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <returns></returns>
        IDbSet<T> DbSet<T>() where T : class;
    }
}