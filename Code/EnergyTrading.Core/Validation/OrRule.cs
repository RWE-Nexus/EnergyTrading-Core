namespace EnergyTrading.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Validates a set of rules which any must be true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrRule<T> : Rule<T>
    {
        private IEnumerable<IRule<T>> Rules { get; set; }

        public OrRule(IEnumerable<IRule<T>> rules, string message = "")
        {
            Rules = rules;
            Message = message;
        }

        /// <inheritdoc />
        public override bool IsValid(T entity)
        {
            var valid = false;
            foreach (var rule in Rules)
            {
                if (rule.IsValid(entity))
                {
                    valid = true;
                    break;
                }
                Message = Message + rule.Message + Environment.NewLine;
            }

            return valid;
        }
    }
}
