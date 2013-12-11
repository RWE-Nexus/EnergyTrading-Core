namespace EnergyTrading.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Exception raised if rule violations 
    /// </summary>
    public class ValidationException : Exception
    {
        private readonly ICollection<IRule> violations;

        public ValidationException(ICollection<IRule> violations)
        {
            this.violations = violations;
        }

        /// <summary>
        /// Gets the collection of rule violations.
        /// </summary>
        public ICollection<IRule> Violations
        {
            get { return this.violations; }
        }

        /// <summary>
        /// Get the violation message.
        /// </summary>
        public override string Message
        {
            get
            {
                return this.violations.Aggregate(string.Empty, (current, rule) => current + (rule.Message + "\r\n"));
            }
        }
    }
}
