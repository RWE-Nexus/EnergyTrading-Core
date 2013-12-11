namespace EnergyTrading.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Validates a set of rules that all must be true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AndRule<T> : Rule<T>
    {
        private IEnumerable<IRule<T>> Rules { get; set; }

        public AndRule(IEnumerable<IRule<T>> rules, string message = "")
        {
            Rules = rules;
            Message = message;
        }

        /// <inheritdoc />
        public override bool IsValid(T entity)
        {
            var valid = true;
            foreach (var rule in Rules.Where(r => !r.IsValid(entity)))
            {
                valid = false;
                Message = Message + rule.Message + Environment.NewLine;
            }
            return valid;
        }
    }
}
