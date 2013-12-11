namespace EnergyTrading.Extensions
{
    using System;

    public static class EnumExtensions
    {
        /// <summary>
        /// Converts a string returning its value in <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return ToEnum(value, default(T));
        }

        /// <summary>
        /// Converts a string returning its value in <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue)
            where T : struct
        {
            return ToEnum(value, defaultValue, true);
        }

        /// <summary>
        /// Converts a string returning its value in <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue, bool ignoreCase)
            where T : struct
        {
            return (T)RawToEnum(value, defaultValue, ignoreCase);
        }

        /// <summary>
        /// Converts multiple string returning the value in <typeparamref name="T"/>
        /// </summary>
        /// <remarks>
        /// The enum <typeparamref name="T"/> enum must have the flags attribute set for this to work
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException" />
        public static T ToEnum<T>(bool ignoreCase, T defaultValue, params string[] values)
            where T : struct
        {
            var result = 0;

            var enumType = typeof(T);
            if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)
            {
                throw new NotSupportedException(string.Format("Enum {0} does not have a [Flags] attribute", enumType.FullName));
            }

            foreach (string value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    result |= (int)RawToEnum(value, defaultValue, ignoreCase);
                }
            }

            // HACK: Can't see another way of doing this - won't allow me to cast int -> T
            return (T)(object)result;
        }

        private static object RawToEnum<T>(string value, T defaultValue, bool ignoreCase)
            where T : struct
        {
            try
            {
                return Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch (ArgumentNullException)
            {
                return defaultValue;
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }
    }
}
