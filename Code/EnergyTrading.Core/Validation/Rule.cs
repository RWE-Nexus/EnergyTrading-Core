namespace EnergyTrading.Validation
{
    /// <summary>
    /// Rule to validate an strongly typed object.
    /// </summary>
    /// <typeparam name="T">Type of the entity to validate.</typeparam>
    public abstract class Rule<T> : IRule<T>
    {
        /// <inheritdoc />
        public string Message { get; protected set; }

        /// <inheritdoc />
        bool IRule.IsValid(object entity)
        {
            return this.IsValid((T)entity);
        }

        /// <inheritdoc />
        public abstract bool IsValid(T entity);
    }
}
