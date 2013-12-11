namespace EnergyTrading.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A rule used for invoking child validators
    /// </summary>
    /// <typeparam name="T">Type we are validating.</typeparam>
    public abstract class ValidatorRule<T> : IRule<T>
    {
        protected ValidatorRule()
        {
            this.Errors = new List<IRule>();
        }

        /// <summary>
        /// Gets the list of rule violations.
        /// </summary>
        public IList<IRule> Errors { get; private set; }

        /// <summary>
        /// Get the error message.
        /// </summary>
        public string Message { get; protected set; }

        /// <inheritdoc />
        public bool IsValid(T entity)
        {
            this.Message = string.Empty;
            this.Errors.Clear();
            
            this.Validate(entity);

            if (this.Errors.Count > 0)
            {
                var m = this.Errors.Aggregate(string.Empty, (current, e) => current + (e.Message + "\r\n"));
                this.Message = m;
            }

            return this.Errors.Count == 0;
        }

        /// <inheritdoc />
        bool IRule.IsValid(object entity)
        {
            return this.IsValid((T)entity);
        }

        protected abstract void Validate(T entity);
    }
}
