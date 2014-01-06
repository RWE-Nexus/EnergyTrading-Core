namespace EnergyTrading.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Validates that a value exists in a supplied set of values.
    /// </summary>
    public class ValueExistsRule<T> : Rule<T>
    {
        private readonly IEnumerable<T> values;
        private readonly IEqualityComparer<T> comparer;

        public ValueExistsRule(IEnumerable<T> set, IEqualityComparer<T> comparer = null)
        {
            this.values = set;
            this.comparer = comparer;
        }

        /// <inheritdoc />
        public override bool IsValid(T entity)
        {
            return (this.comparer == null) ? this.values.Contains(entity) : this.values.Contains(entity, this.comparer);
        }
    }
}