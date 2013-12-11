namespace EnergyTrading.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Return the default value if the source value is null or empty.
        /// </summary>
        /// <param name="source">Value to check</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Original string if not null or empty, value otherwise.</returns>
        public static string DefaultIfNullOrEmpty(this string source, string defaultValue)
        {
            return string.IsNullOrEmpty(source) ? defaultValue : source;
        }

        /// <summary>
        /// Append the value to the source if the source is not null/whitespace and the 
        /// source does not end with the value.
        /// </summary>
        /// <param name="source">Value to check.</param>
        /// <param name="value">Value to append</param>
        /// <returns>Original string if already ends with value or is null/whitespace, original + value otherwise.</returns>
        public static string AppendValueToEndIfMissing(this string source, string value)
        {
            if (!string.IsNullOrWhiteSpace(source) && !source.EndsWith(value))
            {
                return source + value;
            }
            return source;
        }
    }
}
