namespace EnergyTrading.Math
{
    using System;

    /// <summary>
    /// Extension methods for floats.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Calculate the size of an ulp of the specified single-precision floating point number.
        /// <para>
        /// An ulp of a value is the positive difference between this floating point value and the 
        /// value next larger in magnitude. Note that for non-NaN x, ulp(-x) == ulp(x).
        /// </para>
        /// <para>
        /// Special cases:
        /// <list type="bullet">
        /// <item><description>If the argument is NaN, then the result is NaN.</description></item>
        /// <item><description>If the argument is positive or negative infinity then the result is positive infinity</description></item>
        /// <item><description>If the argument is positive or negative zero, then the result is single.MinValue</description></item>
        /// <item><description>If the argument is +-single.MaxValue, then the result is equal to 2^104</description></item>                        
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="value">The double-precision floating point number whose ulp is to be returned.</param>
        /// <returns>Size of the ulp of the argument.</returns>
        /// <remarks>Based on http://stackoverflow.com/questions/9485943/calculate-the-unit-in-the-last-place-ulp-for-doubles</remarks>
        public static float Ulp(this float value)
        {
            if (float.IsNaN(value))
            {
                return value;
            }

            if (float.IsPositiveInfinity(value) || float.IsNegativeInfinity(value))
            {
                return float.PositiveInfinity;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (value == 0)
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                return float.MinValue;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (Math.Abs(value) == double.MaxValue)
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                return (float) Math.Pow(2, 104);
            }

            var bits = value.SingleToInt32Bits();
            bits += 1;
            var nextValue = bits.Int32BitsToSingle();
            var result = nextValue - value;
            return result;
        }

        /// <summary>
        /// Converts the specified single-precision floating point number to a 32-bit signed integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int SingleToInt32Bits(this float value)
        {
            var b = BitConverter.GetBytes(value);
            var i = BitConverter.ToInt32(b, 0);

            return i;
        }

        /// <summary>
        /// Converts the specified 32-bit signed integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Int32BitsToSingle(this int value)
        {
            var b = BitConverter.GetBytes(value);
            var f = BitConverter.ToSingle(b, 0);

            return f;
        }
    }
}