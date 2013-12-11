namespace EnergyTrading.Validation
{
    /// <summary>
    /// Rule to validate an strongly typed object.
    /// </summary>
    /// <typeparam name="T">Type of the entity to validate.</typeparam>
    public interface IRule<in T> : IRule
    {
        /// <summary>
        /// Determine the validity of the entity.
        /// </summary>
        /// <param name="entity">Entity to validate.</param>
        /// <returns>true if the entity is valid, false otherwise.</returns>
        bool IsValid(T entity);
    }
}