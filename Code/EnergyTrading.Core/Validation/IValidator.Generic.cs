namespace EnergyTrading.Validation
{
    using System.Collections.Generic;

    /// <summary>
    /// Validates a strongly-typed object.
    /// </summary>
    /// <typeparam name="T">Type we will validate.</typeparam>
    public interface IValidator<in T> : IValidator
    {
        /// <summary>
        /// Validates an object, recording any rule violations.
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <param name="violations">Rule violation collection</param>
        /// <returns>true if valid, false otherwise.</returns>
        bool IsValid(T entity, IList<IRule> violations);
    }
}