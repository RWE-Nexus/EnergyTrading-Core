namespace EnergyTrading.Validation
{
    /// <summary>
    /// Negates the result of another rule.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotRule<T> : Rule<T>
    {
        private readonly IRule<T> innerRule;

        public NotRule(IRule<T> rule, string message = "")
        {
            this.innerRule = rule;
            this.Message = message;
        }

        /// <inheritdoc />
        public override bool IsValid(T entity)
        {
            return !this.innerRule.IsValid(entity);
        }
    }
}