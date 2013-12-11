namespace EnergyTrading.Validation
{
    using System;

    /// <summary>
    /// Apply a validator to a child entity
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    public class ChildRule<TParent, TChild> : ValidatorRule<TParent>
    {
        private readonly IValidatorEngine validatorEngine;
        private readonly Func<TParent, TChild> accessor;

        /// <summary>
        /// Creates a new ChildRule.
        /// </summary>
        /// <param name="validatorEngine">Validation engine to use</param>
        /// <param name="accessor">Function that returns the child object to validate.</param>
        public ChildRule(IValidatorEngine validatorEngine, Func<TParent, TChild> accessor)
        {
            this.validatorEngine = validatorEngine;
            this.accessor = accessor;
        }

        protected override void Validate(TParent entity)
        {
            var child = this.accessor.Invoke(entity);
            this.validatorEngine.IsValid(child, this.Errors);
        }
    }
}