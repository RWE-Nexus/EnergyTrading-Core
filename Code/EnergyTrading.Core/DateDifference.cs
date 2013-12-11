namespace EnergyTrading
{
    using System;

    /// <summary>
    /// Computes the difference between two dates in years, months and days
    /// </summary>
    public class DateDifference
    {
        /// <summary>
        /// defining Number of days in month; index 0=> january and 11=> December
        /// february contain either 28 or 29 days, that's why here value is -1
        /// which will be calculate later.
        /// </summary>
        private readonly int[] monthDay = new[] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public DateDifference(DateTime start, DateTime finish)
        {
            if (finish < start)
            {
                throw new ArgumentOutOfRangeException("finish", "Finish is less than start date");
            }

            // Day Calculation
            var increment = 0;

            if (start.Day > finish.Day)
            {
                increment = this.monthDay[start.Month - 1];
            }

            // if it is february month
            // if the to day is less then from day
            if (increment == -1)
            {
                // Leap year detection
                increment = DateTime.IsLeapYear(start.Year) ? 29 : 28;
            }

            if (increment != 0)
            {
                this.Days = (finish.Day + increment) - start.Day;
                increment = 1;
            }
            else
            {
                this.Days = finish.Day - start.Day;
            }

            // Month calculation         
            if ((start.Month + increment) > finish.Month)
            {
                this.Months = (finish.Month + 12) - (start.Month + increment);
                increment = 1;
            }
            else
            {
                this.Months = (finish.Month) - (start.Month + increment);
                increment = 0;
            }

            // Year calculation
            this.Years = finish.Year - (start.Year + increment);
        }

        /// <summary>
        /// Gets the number of years difference
        /// </summary>
        public int Years { get; private set; }

        /// <summary>
        /// Gets the number of months difference
        /// </summary>
        public int Months { get; private set; }

        /// <summary>
        /// Gets the number of days difference
        /// </summary>
        public int Days { get; private set; }

        public override string ToString()
        {
            return this.Years + " Year(s), " + this.Months + " month(s), " + this.Days + " day(s)";
        }
    }
}
