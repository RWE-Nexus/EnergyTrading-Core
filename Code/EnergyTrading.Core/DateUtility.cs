namespace EnergyTrading
{
    using System;
    using System.Configuration;

    public static class DateUtility
    {
        static DateUtility()
        {
            MinDate = LoadDefault("Dates.MinDate", new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            MaxDate = LoadDefault("Dates.MaxDate", new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Utc));
            DefaultDate = LoadDefault("Dates.Default", MinDate);
        }

        /// <summary>
        /// A default date to use in the application
        /// </summary>
        /// <remarks>Configured by Dates.Default, and defaults to MinDate if not set</remarks>
        public static DateTime DefaultDate { get; private set; }

        /// <summary>
        /// Gets the MinDate property.
        /// <para>
        /// The minimum business date supported by the system.
        /// </para>
        /// </summary>
        public static DateTime MinDate { get; private set; }

        /// <summary>
        /// Gets the MaxDate property.
        /// <para>
        /// The maximum business date supported by the system.
        /// </para>
        /// </summary>
        public static DateTime MaxDate { get; private set; }

        /// <summary>
        /// Returns a date between <see cref="MinDate" /> and <see cref="MaxDate" /> rounded to database precision.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime Round(DateTime value)
        {
            if (value < MinDate)
            {
                //logger.Debug(string.Format(CultureInfo.InvariantCulture, "Date {0} less than MinDate", value));
                return MinDate;
            }

            if (value > MaxDate)
            {
                //logger.Debug(string.Format(CultureInfo.InvariantCulture, "Date {0} greater than MaxDate", value));
                return MaxDate;
            }

            // Strip the milliseconds - 10000 ticks = 1ms
            const long StripMs = 10000000;
            var ticks = value.Ticks / StripMs;
            var newValue = new DateTime(ticks * StripMs, value.Kind);

            //logger.Debug(string.Format(CultureInfo.InvariantCulture, "Rounded {0} to {1}", value, newValue));

            return newValue;
        }

        public static void SetMinDate(DateTime value)
        {
            MinDate = value;
        }

        public static void SetMaxDate(DateTime value)
        {
            MaxDate = value;
        }

        private static DateTime LoadDefault(string dateName, DateTime defaultValue)
        {
            DateTime value;
            var dv = ConfigurationManager.AppSettings[dateName];
            if (DateTime.TryParse(dv, out value))
            {
                //logger.Info(string.Format(CultureInfo.InvariantCulture, "Acquired date {0} with value {1}", dateName, dv));
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
                return value;
            }

            //logger.Info(
            //    string.Format(
            //        CultureInfo.InvariantCulture,
            //        "No date found for {0} setting to default {1}",
            //        dateName,
            //        defaultValue));
            return defaultValue;
        }
    }
}