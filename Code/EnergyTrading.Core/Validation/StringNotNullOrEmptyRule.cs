namespace EnergyTrading.Validation
{
    /// <summary>
    /// Validates whether a string is null or empty and optionally just whitespace.
    /// </summary>
    public class StringNotNullOrEmptyRule : Rule<string>
    {
        /// <summary>
        /// Get or set whether whitespace causes a rule violation.
        /// </summary>
        public bool AllowWhiteSpace { get; set; }

        /// <inheritdoc />
        public override bool IsValid(string entity)
        {
            var valid = this.AllowWhiteSpace ? !string.IsNullOrEmpty(entity) : !string.IsNullOrWhiteSpace(entity);
            if (!valid)
            {
                this.Message = "value is null or " + (this.AllowWhiteSpace ? "empty" : "whitespace");
            }
            return valid;
        }
    }
}
