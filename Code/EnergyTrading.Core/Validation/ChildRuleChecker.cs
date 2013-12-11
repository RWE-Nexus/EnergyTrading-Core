namespace EnergyTrading.Validation
{
    using System;

    public class ChildRuleChecker<TParent, TChild> : Rule<TParent>
        where TParent : class
    {
        private readonly IRule<TChild> rule;
        private readonly Func<TParent, TChild> accessor;
        private readonly string prefix;

        public ChildRuleChecker(IRule<TChild> ruleToCheck, Func<TParent, TChild> accessor, string messagePrefix = "")
        {
            this.accessor = accessor;
            rule = ruleToCheck;
            prefix = messagePrefix;
        }

        /// <inheritdoc />
        public override bool IsValid(TParent entity)
        {
            var child = accessor.Invoke(entity);
            var valid = rule.IsValid(child);
            if (!valid)
            {
                Message = ((prefix ?? string.Empty) + " " + rule.Message).TrimStart();
            }

            return valid;
        }
    }
}