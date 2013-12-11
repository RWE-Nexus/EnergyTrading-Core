namespace EnergyTrading.UnitTest
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DateRangeFixture
    {
        private readonly DateTime date1900 = new DateTime(1900, 1, 1);
        private readonly DateTime date2000 = new DateTime(2000, 1, 1);
        private readonly DateTime date1950 = new DateTime(1950, 6, 20);

        [TestMethod]
        public void ContainsDate()
        {
            var range = new DateRange(this.date1900, this.date2000);
            Assert.AreEqual(this.date1900, range.Start, "Start");
            Assert.AreEqual(this.date2000, range.Finish, "Finish");

            Assert.IsTrue(range.Contains(this.date1900), "1900");
            Assert.IsTrue(range.Contains(this.date1950), "1950");
            Assert.IsTrue(range.Contains(this.date2000), "2000");
        }

        [TestMethod]
        public void Equals()
        {
            var first = new DateRange(date1900, date2000);
            var second = new DateRange(date1900, date2000);

            Assert.IsTrue(first == second, "Operator equality");
            Assert.IsTrue(first.Equals(second), "Method equality");
        }

        [TestMethod]
        public void NotEquals()
        {
            var first = new DateRange(date1900, date2000);
            var second = new DateRange(date1900, date1950);

            Assert.IsTrue(first != second, "Operator inequality");
            Assert.IsFalse(first.Equals(second), "Method inequality");
        }

        [TestMethod]
        public void ChangeStart()
        {
            var first = new DateRange(date1900, date2000);

            Assert.AreEqual(new DateRange(date1950, date2000), first.ChangeStart(date1950), "ChangeStart Date");
            Assert.AreEqual(new DateRange(date1900.AddDays(-1), date2000), first.ChangeStart(new TimeSpan(-1, 0, 0, 0)), "ChangeStart TimeSpan");
        }

        [TestMethod]
        public void ChangeFinish()
        {
            var first = new DateRange(date1900, date2000);

            Assert.AreEqual(new DateRange(date1900, date1950), first.ChangeFinish(date1950), "ChangeFinish Date");
            Assert.AreEqual(new DateRange(date1900, date2000.AddDays(-1)), first.ChangeFinish(new TimeSpan(-1, 0, 0, 0)), "ChangeFinish TimeSpan");
        }

        [TestMethod]
        public void ContainsRange()
        {
            var first = new DateRange(date1900, date2000);
            var second = new DateRange(date1950, date1950);

            Assert.IsTrue(first.Contains(second));
        }

        [TestMethod]
        public void Union()
        {
            var first = new DateRange(date1900, date1950);
            var second = new DateRange(date1950, date2000);

            Assert.AreEqual(new DateRange(date1900, date2000), first.Union(second));
        }

        [TestMethod]
        public void NotContainsRange()
        {
            var first = new DateRange(date1900, date2000);
            var second = new DateRange(date1900.AddDays(-1), date1950);

            Assert.IsFalse(first.Contains(second));
        }

        [TestMethod]
        public void Disjoint()
        {
            var first = new DateRange(date1900, date1950.AddSeconds(-1));
            var second = new DateRange(date1950, date2000);

            Assert.IsTrue(first.Disjoint(second));
        }

        [TestMethod]
        public void NotDisjoint()
        {
            var first = new DateRange(date1900, date1950.AddSeconds(1));
            var second = new DateRange(date1950, date2000);

            Assert.IsFalse(first.Disjoint(second));
        }

        [TestMethod]
        public void UnspecifiedToUtcSKeepsTime()
        {
            var start = new DateTime(2009, 6, 1, 12, 0, 0);
            var finish = new DateTime(2009, 7, 1, 12, 0, 0);

            var dr = new DateRange(start, finish, DateTimeKind.Utc);
            Assert.AreEqual(12, dr.Start.Hour, "Hour differs");
        }

        [TestMethod]
        public void UnspecifiedToLocalKeepsTime()
        {
            var start = new DateTime(2009, 6, 1, 12, 0, 0);
            var finish = new DateTime(2009, 7, 1, 12, 0, 0);

            var dr = new DateRange(start, finish, DateTimeKind.Local);
            Assert.AreEqual(12, dr.Start.Hour, "Hour differs");
        }

        [TestMethod]
        public void DateRangeStateComparisons()
        {
            var start = new DateTime(2009, 6, 1, 12, 0, 0);
            var finish = new DateTime(2009, 7, 1, 12, 0, 0);

            var dr = new DateRange(start, finish);

            Assert.AreEqual(DateRangeState.Current, dr.DetermineState(start.AddDays(1)), "Current");
            Assert.AreEqual(DateRangeState.Past, dr.DetermineState(start.AddDays(-1)), "Past check");
            Assert.AreEqual(DateRangeState.Future, dr.DetermineState(finish.AddDays(1)), "Future check");
        }

        [TestMethod]
        public void DateRangeNotExceedingMinMax()
        {
            var oldMin = DateUtility.MinDate;
            var oldMax = DateUtility.MaxDate;

            DateUtility.SetMinDate(new DateTime(1753, 1, 1, 0, 0, 0));
            DateUtility.SetMaxDate(new DateTime(2020, 12, 31, 0, 0, 0));

            var range = new DateRange(DateTime.MinValue, DateTime.MaxValue);
            Assert.AreEqual(DateUtility.MinDate, range.Start, "Start differs");
            Assert.AreEqual(DateUtility.MaxDate, range.Finish, "Finish differs");

            DateUtility.SetMinDate(oldMin);
            DateUtility.SetMaxDate(oldMax);
        }

        [TestMethod]
        public void ValueNotEqualToNull()
        {
            var start = new DateTime(2011, 1, 1);
            var finish = new DateTime(2011, 12, 31);
            var value = new DateRange(start, finish);

            Assert.IsFalse(value == null);
        }
    }
}