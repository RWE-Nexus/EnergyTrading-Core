namespace EnergyTrading.Validation
{
    using System.Collections.Generic;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Uses ServiceLocator to implement a validator engine.
    /// </summary>
    public class LocatorValidatorEngine : IValidatorEngine
    {
        private readonly IServiceLocator locator;

        public LocatorValidatorEngine(IServiceLocator locator)
        {
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
            return this.locator.GetInstance<IValidator<T>>();
        }
    }
}