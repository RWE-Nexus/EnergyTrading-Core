namespace EnergyTrading.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DateRangeExtensions
    {
        public static bool ValidAt(this DateRange range, DateTime value)
        {
            return range.DetermineState(value) == DateRangeState.Current;
        }

        public static int DaysDuration(this IEnumerable<DateRange> ranges)
        {
            return ranges.Sum(range => DaysDuration(range));
        }

        public static int DaysDuration(this DateRange range)
        {
            var span = range.Finish - range.Start;

            return span.Days;
        }
    }
}
