namespace EnergyTrading.Validation
{
    /// <summary>
    /// Validate max length of a string.
    /// </summary>
    public class StringLengthRule : Rule<string>
    {
        public StringLengthRule(int maxLength)
        {
            MaxLength = maxLength;
            Message = "Value exceeds " + maxLength + " characters";
        }

        public int MaxLength { get; set; }

        /// <inheritdoc />
        public override bool IsValid(string entity)
        {
            var valid = string.IsNullOrEmpty(entity) || entity.Length <= MaxLength;

            return valid;
        }
    }
}
