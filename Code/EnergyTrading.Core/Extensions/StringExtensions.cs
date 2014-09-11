namespace EnergyTrading.Extensions
{
    using System;
    using System.Linq;

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

        /// <summary>
        /// checks to see if a supplied string matches with any item in the list
        /// allows for the following simple logic all currently case-insensitive
        /// * or *.* => match all items
        /// *xxx or *.xxx => EndsWith("xxx")
        /// xxx* or xxx.* => StartsWith("xxx")
        /// *xxx* or *.xxx.* => Contains("xxx")
        /// xxx => string.Compare() == 0
        /// </summary>
        /// <param name="validValues">the list of expressions to match against</param>
        /// <param name="itemToMatch">the string to match</param>
        /// <returns>true if the string matches an item in the list. False if the sting is not matched, the array null is empty or the supplied string is null or white space</returns>
        public static bool Matches(this string[] validValues, string itemToMatch)
        {
            int matchedCharacters;
            return Matches(validValues, itemToMatch, out matchedCharacters);
        }

        /// <summary>
        /// checks to see if a supplied string matches with any item in the list
        /// allows for the following simple logic all currently case-insensitive
        /// * or *.* => match all items
        /// *xxx or *.xxx => EndsWith("xxx")
        /// xxx* or xxx.* => StartsWith("xxx")
        /// *xxx* or *.xxx.* => Contains("xxx")
        /// xxx => string.Compare() == 0
        /// </summary>
        /// <param name="validValues">the list of expressions to match against</param>
        /// <param name="itemToMatch">the string to match</param>
        /// <param name="noOfMatchedCharacters">out parameter that returns the number of characters that was actually matched</param>
        /// <returns>true if the string matches an item in the list. False if the sting is not matched, the array null is empty or the supplied string is null or white space</returns>
        public static bool Matches(this string[] validValues, string itemToMatch, out int noOfMatchedCharacters)
        {
            if (string.IsNullOrWhiteSpace(itemToMatch) || validValues == null || validValues.Length == 0)
            {
                noOfMatchedCharacters = 0;
                return false;
            }

            if (validValues.Contains("*") || validValues.Contains("*.*"))
            {
                noOfMatchedCharacters = itemToMatch.Length;
                return true;
            }

            var lowerCode = itemToMatch.ToLowerInvariant();
            foreach (var handledCode in validValues)
            {
                var lowerHandledCode = handledCode.ToLowerInvariant();
                if (string.Compare(handledCode, itemToMatch, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    noOfMatchedCharacters = itemToMatch.Length;
                    return true;
                }
                var leftTrim = 0;
                var rightTrim = 0;
                if (handledCode.StartsWith("*."))
                {
                    leftTrim = 2;
                }
                else if (handledCode.StartsWith("*"))
                {
                    leftTrim = 1;
                }

                if (handledCode.EndsWith(".*"))
                {
                    rightTrim = 2;
                }
                else if (handledCode.EndsWith("*"))
                {
                    rightTrim = 1;
                }
                if (leftTrim > 0 && rightTrim > 0)
                {
                    var testString = lowerHandledCode.Substring(leftTrim, lowerHandledCode.Length - (leftTrim + rightTrim));
                    if (lowerCode.Contains(testString))
                    {
                        noOfMatchedCharacters = testString.Length;
                        return true;
                    }
                }
                if (leftTrim > 0)
                {
                    var testString = lowerHandledCode.Substring(leftTrim);
                    if (lowerCode.EndsWith(testString))
                    {
                        noOfMatchedCharacters = testString.Length;
                        return true;
                    }
                }
                if (rightTrim > 0)
                {
                    var testString = lowerHandledCode.Substring(0, lowerHandledCode.Length - rightTrim);
                    if (lowerCode.StartsWith(testString))
                    {
                        noOfMatchedCharacters = testString.Length;
                        return true;
                    }
                }
            }
            noOfMatchedCharacters = 0;
            return false;
        }
    }
}
