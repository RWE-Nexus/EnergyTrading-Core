namespace EnergyTrading
{
    using System;

    /// <summary>
    /// Represents a period of time
    /// </summary>
    /// <remarks>Only a class due to EF!</remarks>
    [Serializable]
    public class DateRange
    {
        private static readonly DateRange noValueDateRange = new DateRange();
        private readonly bool hasValue;

        private DateTime start;
        private DateTime finish;

        public DateRange() : this(DateUtility.MinDate, DateUtility.MaxDate)
        {            
        }

        public DateRange(DateTime? start, DateTime? finish) : this(
            start.HasValue ? start.Value : DateUtility.MinDate, 
            finish.HasValue ? finish.Value : DateUtility.MaxDate)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the DateRange class.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="span"></param>
        public DateRange(DateTime start, TimeSpan span)
            : this(start, start.Add(span))
        {
        }

        /// <summary>
        /// Initializes a new instance of the DateRange class.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        public DateRange(DateTime start, DateTime finish)
            : this(start, finish, start.Kind)
        {
        }

        public DateRange(DateTime start, DateTime finish, DateTimeKind kind)
        {
            start = DateUtility.Round(start);
            finish = DateUtility.Round(finish);

            if (start > finish)
            {
                throw new ArgumentOutOfRangeException("finish", "Finish must be after start");
            }

            // Force to the specified Kind
            this.start = DateTime.SpecifyKind(start, kind);
            this.finish = DateTime.SpecifyKind(finish, kind);

            // NB This is probably broken by EF as it won't use our constructors
            this.hasValue = true;
        }

        /// <summary>
        /// Get the duration of the date range
        /// </summary>
        public TimeSpan Duration
        {
            get { return this.finish.Subtract(this.start); }
        }

        /// <summary>
        /// A date range that has no value.
        /// </summary>
        public static DateRange NoValueDateRange
        {
            get { return noValueDateRange; }
        }

        /// <summary>
        /// A DateRange that is is the largest that we can represent.
        /// </summary>
        public static DateRange MaxDateRange
        {
            get { return new DateRange(DateUtility.MinDate, DateUtility.MaxDate); }
        }

        /// <summary>
        /// Gets the start DateTime of the period.
        /// </summary>
        public DateTime Start
        {
            get { return this.start; }
            private set { this.start = value; }
        }

        /// <summary>
        /// Gets the end DateTime of the period.
        /// </summary>
        public DateTime Finish
        {
            get { return this.finish; }
            private set { this.finish = value; }
        }

        /// <summary>
        /// Gets the DateTimeKind of the range
        /// </summary>
        public DateTimeKind Kind
        {
            get { return this.start.Kind; }
        }

        /// <summary>
        /// Gets whether the period has a value.
        /// </summary>
        public bool HasValue
        {
            get { return this.hasValue; }
        }

        public static bool operator ==(DateRange v1, DateRange v2)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(v1, v2))
            {
                return true;
            }

            // if the LHS is null, return false, otherwise call the field level equals
            return (object) v1 != null && v1.Equals(v2);
        }

        public static bool operator !=(DateRange v1, DateRange v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Create a new DateRange object that represents the same period but is designated in either local or UTC time.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="kind"></param>
        public static DateRange SpecifyKind(DateRange range, DateTimeKind kind)
        {
            return new DateRange(range.start.ToUniversalTime(), range.finish.ToUniversalTime(), kind);
        }

        /// <summary>
        /// Returns a new <see cref="DateRange" /> with the start date modified by the
        /// specified amount and the original finish date.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateRange ChangeStart(TimeSpan value)
        {
            return new DateRange(this.Start.Add(value), this.Finish, this.Kind);
        }

        /// <summary>
        /// Returns a new <see cref="DateRange" /> with the specified start date and the 
        /// original finish date.
        /// </summary>
        /// <param name="value">The new start date</param>
        /// <returns></returns>
        public DateRange ChangeStart(DateTime value)
        {
            return new DateRange(value, this.Finish, this.Kind);
        }

        /// <summary>
        /// Returns a new <see cref="DateRange" /> with the original start date and the 
        /// specified finish date.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateRange ChangeFinish(DateTime value)
        {
            return new DateRange(this.Start, value, this.Kind);
        }

        /// <summary>
        /// Returns a new <see cref="DateRange" /> with the original start date and the 
        /// finish date modified by the specified amount.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateRange ChangeFinish(TimeSpan value)
        {
            return new DateRange(this.Start, this.Finish.Add(value), this.Kind);
        }

        /// <summary>
        /// Is the value contiguous to the original within 1 second.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contiguous(DateRange value)
        {
            return this.Contiguous(value, new TimeSpan(0, 0, 0, 1));
        }

        /// <summary>
        /// Is the value contiguous to the original within the specified interval
        /// </summary>
        /// <param name="value"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool Contiguous(DateRange value, TimeSpan interval)
        {
            // Must be strictly after this range (to get a positive difference) and be less than the specified interval
            return value.Start > this.Finish && (value.Start - this.Finish) < interval;
        }

        /// <summary>
        /// Return the difference in years, months and days
        /// </summary>
        /// <returns></returns>
        public DateDifference Difference()
        {
            return new DateDifference(this.start, this.finish);
        }

        /// <summary>
        /// Returns the union of the two period.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// this when !value.HasValue
        /// value when !this.HasValue
        /// NoValueDateRange when this.Disjoint(value)
        /// the union value otherwise.
        /// </returns>
        public DateRange Union(DateRange value)
        {
            if (!this.HasValue)
            {
                return value.HasValue ? new DateRange(value.Start, value.Finish) : NoValueDateRange;
            }

            if (!value.HasValue)
            {
                return this;
            }

            if (this.Disjoint(value))
            {
                return NoValueDateRange;
            }

            var newStart = value.Start < this.Start ? value.Start : this.Start;
            var newFinish = value.Finish > this.Finish ? value.Finish : this.Finish;

            return new DateRange(newStart, newFinish, this.Kind);
        }

        /// <summary>
        /// Returns the intersection of the two periods.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>the intersecting period or <see cref="NoValueDateRange" /> if either do not have a value.</returns>
        public DateRange Intersect(DateRange value)
        {
            if (!this.HasValue || !value.HasValue || this.Disjoint(value))
            {
                return NoValueDateRange;
            }

            var newStart = value.Start > this.Start ? value.Start : this.Start;
            var newFinish = value.Finish < this.Finish ? value.Finish : this.Finish;

            return new DateRange(newStart, newFinish, this.Kind);
        }

        /// <summary>
        /// Is the date contained within the <see cref="DateRange" />
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(DateTime value)
        {
            if (!this.HasValue)
            {
                return false;
            }

            //DateTime.SpecifyKind(value, Kind);
            return (this.start <= value) && (value <= this.finish);
        }

        /// <summary>
        /// Is the period wholly contained within the <see cref="DateRange" />
        /// The end points of the period are considered to be in the period.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(DateRange value)
        {
            if (!this.HasValue || !value.HasValue)
            {
                return false;
            }

            //value = new DateRange(value.Start, value.Finish, Kind);
            return this.Start <= value.Start && value.Finish <= this.Finish;
        }

        /// <summary>
        /// Do the two periods overlap at all.
        /// </summary>
        /// <remarks>
        /// We can have both <c>!period.Disjoint(value)</c> and <c>!period.Overlaps(value)</c> if
        /// either of period or value have !HasValue.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>true if the periods overlap, false otherwise</returns>
        public bool Overlaps(DateRange value)
        {
            // NB Need to duplicate this test so we return false for !HasValue
            if (!this.HasValue || !value.HasValue)
            {
                return false;
            }

            //value = new DateRange(value.Start, value.Finish, Kind);
            return !this.Disjoint(value);
        }

        /// <summary>
        /// Are the two periods entirely disjoint.
        /// </summary>
        /// <remarks>
        /// We can have both <c>!period.Disjoint(value)</c> and <c>!period.Overlaps(value)</c> if
        /// either of period or value have !HasValue.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>true if the periods do not overlap, false otherwise.</returns>
        public bool Disjoint(DateRange value)
        {
            if (!this.HasValue || !value.HasValue)
            {
                return false;
            }

            //value = new DateRange(value.Start, value.Finish, Kind);
            return value.Start > this.Finish || value.Finish < this.Start;
        }

        /// <summary>
        /// Calculate the state of the current time with respect to the date range.
        /// </summary>
        /// <returns>The <see cref="DateRangeState" /> that applies.</returns>
        public DateRangeState DetermineState()
        {
            return this.DetermineState(SystemTime.UtcNow());
        }

        /// <summary>
        /// Calculate the state of a specified date with respect to the date range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The <see cref="DateRangeState" /> that applies.</returns>
        public DateRangeState DetermineState(DateTime value)
        {
            if (!this.HasValue)
            {
                return DateRangeState.Unknown;
            }

            if (this.Start > value)
            {
                return DateRangeState.Past;
            }

            return value <= this.Finish ? DateRangeState.Current : DateRangeState.Future;
        }

        /// <summary>
        /// Return whether the values are equal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Equals(object value)
        {
            return value != null && this.Equals((DateRange)value);
        }

        /// <summary>
        /// Return whether the values are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>false if other is null, otherwise true if Start/Finish are equal, otherwise false</returns>
        public bool Equals(DateRange other)
        {
            if ((object) other == null)
            {
                return false;
            }
                
            return (this.Start == other.Start) && (this.Finish == other.Finish);
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Start.GetHashCode() ^ this.Finish.GetHashCode();
        }

        public override string ToString()
        {
            return this.start.ToString() + " - " + this.finish.ToString();
        }

        public string ToString(string format)
        {
            return this.start.ToString(format) + " - " + this.finish.ToString(format);
        }
    }
}