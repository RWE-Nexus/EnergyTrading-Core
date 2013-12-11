namespace EnergyTrading.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Validates a strongly-typed object.
    /// </summary>
    /// <typeparam name="T">Type we will validate.</typeparam>
    public class Validator<T> : IValidator<T>
    {
        public Validator()
        {
            this.Rules = new List<IRule>();
        }

        /// <summary>
        /// Gets the Rules property.
        /// <para>
        /// The rules that this validator will enforce.
        /// </para>
        /// </summary>
        public IList<IRule> Rules { get; private set; }

        /// <inheritdoc />
        public bool IsValid(T entity, IList<IRule> violations)
        {
            var valid = true;
            foreach (var rule in this.Rules.Where(rule => !rule.IsValid(entity)))
            {
                valid = false;
                violations.Add(rule);
            }

            return valid;
        }

        /// <inheritdoc />
        bool IValidator.IsValid(object entity, IList<IRule> violations)
        {
            return this.IsValid((T)entity, violations);
        }
    }
}