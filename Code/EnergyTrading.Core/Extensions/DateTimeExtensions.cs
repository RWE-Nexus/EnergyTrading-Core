namespace EnergyTrading.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Delivery the next date of the specified DayOfWeek after the date.
        /// </summary>
        /// <param name="value">Datetime to use</param>
        /// <param name="dayOfWeek">DayOfWeek to use</param>
        /// <returns></returns>
        public static DateTime NextDateForDay(this DateTime value, DayOfWeek dayOfWeek)
        {
            return value.AddDays(value.DaysToAdd(dayOfWeek));
        }

        /// <summary>
        /// Return the number of days in the month of the date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        /// <summary>
        /// Determine how many days to add to a date to get to the next specified day of the week.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int DaysToAdd(this DateTime value, DayOfWeek target)
        {
            var s = (int)value.DayOfWeek;
            var t = (int)target;
            var n = t - s;
            if (n <= 0)
            {
                n += 7;
            }

            return n;
        }

        /// <summary>
        /// Return true, if the date is beginning of month, i.e.: 1st of any month
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBeginningOfMonth(this DateTime value)
        {
            return value.Day == 1;
        }

        /// <summary>
        /// Return true, if the date is end of the month, i.e. 28/29/30/31 depends on the month
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEndOfMonth(this DateTime value)
        {
            return value.AddDays(1).Day == 1;
        }
    }
}