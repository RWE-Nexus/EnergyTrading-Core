namespace EnergyTrading.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Applies a validator to each member of a child collection
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    public class ChildCollectionRule<TParent, TChild> : ValidatorRule<TParent>
    {
        private readonly IValidatorEngine validatorEngine;
        private readonly Func<TParent, IEnumerable<TChild>> accessor;

        public ChildCollectionRule(IValidatorEngine validatorEngine, Func<TParent, IEnumerable<TChild>> accessor)
        {
            this.validatorEngine = validatorEngine;
            this.accessor = accessor;
        }

        protected override void Validate(TParent entity)
        {
            foreach (var child in this.accessor.Invoke(entity))
            {
                this.validatorEngine.IsValid(child, this.Errors);
            }
        }
    }
}