namespace EnergyTrading.Validation
{
    /// <summary>
    /// Rule to validate an object.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Gets the validation message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Determine the validity of the entity.
        /// </summary>
        /// <param name="entity">Entity to validate.</param>
        /// <returns>true if the entity is valid, false otherwise.</returns>
        bool IsValid(object entity);
    }
}