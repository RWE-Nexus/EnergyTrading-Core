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
            this.rule = ruleToCheck;
            this.prefix = messagePrefix;
        }

        /// <inheritdoc />
        public override bool IsValid(TParent entity)
        {
            var child = this.accessor.Invoke(entity);
            var valid = this.rule.IsValid(child);
            if (!valid)
            {
                this.Message = ((this.prefix ?? string.Empty) + " " + this.rule.Message).TrimStart();
            }

            return valid;
        }
    }
}