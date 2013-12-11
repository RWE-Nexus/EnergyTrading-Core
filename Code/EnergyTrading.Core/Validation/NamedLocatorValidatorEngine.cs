namespace EnergyTrading.Validation
{
    using System.Collections.Generic;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Uses ServiceLocator to implement a validator engine.
    /// <para>
    /// Uses a named validator for each type, which can be used to support
    /// more complicated validation scenarios, e.g. validators that depend 
    /// on the state of the entity being validated.
    /// </para>
    /// </summary>
    public class NamedLocatorValidatorEngine : IValidatorEngine
    {
        private readonly string name;
        private readonly IServiceLocator locator;

        public NamedLocatorValidatorEngine(string name, IServiceLocator locator)
        {
            this.name = name;
            this.locator = locator;
        }

        /// <inheritdoc />
        public bool IsValid<T>(T entity, IList<IRule> violations)
        {
            return this.Validator<T>().IsValid(entity, violations);
        }

        /// <summary>
        /// Gets a validator for an entity.
        /// </summary>
        /// <typeparam name="T">Type of the entity we want.</typeparam>
        /// <returns>A validator.</returns>
        public IValidator<T> Validator<T>()
        {
            return this.locator.GetInstance<IValidator<T>>(name);
        }
    }
}