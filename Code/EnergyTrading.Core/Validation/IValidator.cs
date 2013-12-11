namespace EnergyTrading.Validation
{
    using System.Collections.Generic;

    /// <summary>
    /// Validates an object.
    /// </summary>
    /// <remarks>
    /// This interface is deliberately designed around taking <see cref="object" /> as
    /// it can be called in scenarios where we do not have, or want to pass, a strongly-typed
    /// interface.
    /// </remarks>
    /// <seealso cref="IValidator{T}"/>
    public interface IValidator
    {
        /// <summary>
        /// Validates an object, recording any rule violations.
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <param name="violations">Rule violation collection</param>
        /// <returns>true if valid, false otherwise.</returns>        
        bool IsValid(object entity, IList<IRule> violations);
    }
}