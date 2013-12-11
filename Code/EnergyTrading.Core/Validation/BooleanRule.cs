namespace EnergyTrading.Validation
{
    /// <summary>
    /// Validates a boolean value.
    /// </summary>
    public class BooleanRule : Rule<bool>
    {
        /// <inheritdoc />
        public override bool IsValid(bool entity)
        {
            return entity;
        }
    }
}
