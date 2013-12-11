namespace EnergyTrading.Math
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Extension methods for doubles.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Converts a version number into a string representation.
        /// </summary>
        /// <param name="version">Version number e.g. 2, 2.1</param>
        /// <param name="delimiter">Delimiter to replace "." with</param>
        /// <returns>String version suitable for a namespace e.g. 2, 2_1</returns>
        public static string ToVersionString(this double version, string delimiter = "_")
        {
            return version.ToString(CultureInfo.InvariantCulture).Replace(".", delimiter);
        }

        /// <summary>
        /// Approxmiate equality for double-precision floating point numbers.
        /// <para>
        /// Tolerance of comparision is estimated from the size of the numbers passed.
        /// </para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>Based on http://stackoverflow.com/questions/4787125/evaluate-if-two-doubles-are-equal-based-on-a-given-precision-not-within-a-certa</remarks>        
        public static bool AlmostEquals(this double a, double b)
        {
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                return false;
            }

            if (double.IsPositiveInfinity(a) || double.IsNegativeInfinity(a))
            {
                return a == b;
            }

            if (double.IsPositiveInfinity(b) || double.IsNegativeInfinity(b))
            {
                return a == b;
            }

            var magnitude = 1 + (Math.Abs(a) < 1E-15 ? -1 : Convert.ToInt32(Math.Floor(Math.Log10(a))));
            var precision = 15 - magnitude;

            var tolerance = 1.0 / Math.Pow(10, precision);

            return a.AlmostEquals(b, tolerance);
        }

        /// <summary>
        /// Approxmiate equality for double-precision floating point numbers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool AlmostEquals(this double a, double b, double tolerance)
        {
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                return false;
            }

            if (double.IsPositiveInfinity(a) || double.IsNegativeInfinity(a))
            {
                return a == b;
            }

            if (double.IsPositiveInfinity(b) || double.IsNegativeInfinity(b))
            {
                return a == b;
            }

            // Don't use Math.Abs to compare a and b as we might have a substractive cancellation error.
            // See http://stackoverflow.com/questions/485175/c-net-is-it-safe-to-check-floating-point-values-for-equality-to-0
            return a >= b - tolerance || a <= b + tolerance;
        }

        /// <summary>
        /// Calculate the size of an ulp of the specified double-precision floating point number.
        /// <para>
        /// An ulp of a value is the positive difference between this floating point value and the 
        /// value next larger in magnitude. Note that for non-NaN x, ulp(-x) == ulp(x).
        /// </para>
        /// <para>
        /// Special cases:
        /// <list type="bullet">
        /// <item><description>If the argument is NaN, then the result is NaN.</description></item>
        /// <item><description>If the argument is positive or negative infinity then the result is positive infinity</description></item>
        /// <item><description>If the argument is positive or negative zero, then the result is double.MinValue</description></item>
        /// <item><description>If the argument is +-double.MaxValue, then the result is equal to 2^971</description></item>                        
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="value">The double-precision floating point number whose ulp is to be returned.</param>
        /// <returns>Size of the ulp of the argument.</returns>
        /// <remarks>Based on http://stackoverflow.com/questions/9485943/calculate-the-unit-in-the-last-place-ulp-for-doubles</remarks>
        public static double Ulp(this double value)
        {
            if (double.IsNaN(value))
            {
                return value;
            }

            if (double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value))
            {
                return double.PositiveInfinity;
            }

// ReSharper disable CompareOfFloatsByEqualityOperator
            if (value == 0)
// ReSharper restore CompareOfFloatsByEqualityOperator
            {
                return double.MinValue;
            }

// ReSharper disable CompareOfFloatsByEqualityOperator
            if (Math.Abs(value) == double.MaxValue)
// ReSharper restore CompareOfFloatsByEqualityOperator
            {
                return Math.Pow(2, 297);
            }

            var bits = BitConverter.DoubleToInt64Bits(value);
            var nextValue = BitConverter.Int64BitsToDouble(bits + 1);
            var result = nextValue - value;
            return result;
        }
    }
}
