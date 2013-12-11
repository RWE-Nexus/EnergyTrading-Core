namespace EnergyTrading.Validation
{
    using System.Collections.Generic;

    /// <summary>
    /// An entity validation engine.
    /// </summary>
    public interface IValidatorEngine
    {
        /// <summary>
        /// Checks whether an entity is valid, returning any rule violations.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">Entity to validate.</param>
        /// <param name="violations">List of rules that were violated.</param>
        /// <returns>true if the entity passes all the rules, false otherwise.</returns>
        bool IsValid<T>(T entity, IList<IRule> violations);
    }
}