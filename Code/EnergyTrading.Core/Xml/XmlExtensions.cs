namespace EnergyTrading.Xml
{
    using System;

    /// <summary>
    /// Extensions to System.Xml
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Standard XML date format.
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Standard XML time format to second precision.
        /// </summary>
        public const string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// Standard XML time format to millisecond precision.
        /// </summary>
        public const string TimeFormatMillisecond = "HH:mm:ss.fff";

        /// <summary>
        /// Standard XML UTC date time format to millisecond precision.
        /// </summary>
        public const string UtcDateTimeMillisecondFormat = DateFormat + "'T'" + TimeFormatMillisecond + "'Z'";

        /// <summary>
        /// Standard XML UTC date time format to second precision.
        /// </summary>
        public const string UtcDateTimeSecondFormat = DateFormat + "'T'" + TimeFormat + "'Z'";

        /// <summary>
        /// Determine whether we should include milliseconds in the format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UtcDateTimeFormat(this DateTime value)
        {
            return value.Millisecond != 0 ? UtcDateTimeMillisecondFormat : UtcDateTimeSecondFormat;
        }
    }
}
