namespace EnergyTrading.Validation
{
    /// <summary>
    /// Validate max length of a string.
    /// </summary>
    public class StringLengthRule : Rule<string>
    {
        public StringLengthRule(int maxLength)
        {
            this.MaxLength = maxLength;
            this.Message = "Value exceeds " + maxLength + " characters";
        }

        public int MaxLength { get; set; }

        /// <inheritdoc />
        public override bool IsValid(string entity)
        {
            var valid = string.IsNullOrEmpty(entity) || entity.Length <= this.MaxLength;

            return valid;
        }
    }
}
