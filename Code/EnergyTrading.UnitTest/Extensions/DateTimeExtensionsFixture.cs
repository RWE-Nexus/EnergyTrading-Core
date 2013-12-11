namespace EnergyTrading.UnitTest.Extensions
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Extensions;

    [TestClass]
    public class DateTimeExtensionsFixture
    {
        [TestMethod]
        public void ShouldReturnTrueAsEndOfMonthIfMonthHasOnly28Days()
        {
            Assert.IsTrue(new DateTime(2011, 02, 28).IsEndOfMonth());
        }

        [TestMethod]
        public void ShouldReturnTrueAsEndOfMonthIfMonthHasOnly29Days()
        {
            Assert.IsTrue(new DateTime(2012, 02, 29).IsEndOfMonth());
        }

        [TestMethod]
        public void ShouldReturnTrueAsEndOfMonthIfMonthHasOnly30Days()
        {
            Assert.IsTrue(new DateTime(2012, 04, 30).IsEndOfMonth());
        }

        [TestMethod]
        public void ShouldReturnTrueAsEndOfMonthIfMonthHasOnly31Days()
        {
            Assert.IsTrue(new DateTime(2012, 01, 31).IsEndOfMonth());
        }

        [TestMethod]
        public void ShouldReturnTrueAsBeginningOfMonthOnlyForFirstDayOfMonth()
        {
            Assert.IsTrue(new DateTime(2012, 01, 01).IsBeginningOfMonth());
            Assert.IsTrue(new DateTime(2012, 02, 01).IsBeginningOfMonth());
            Assert.IsTrue(new DateTime(2012, 12, 01).IsBeginningOfMonth());
        }

        [TestMethod]
        public void ShouldReturnFalseAsBeginningOfMonthForAnyDayOfMonthOtherThanFirstDay()
        {
            Assert.IsFalse(new DateTime(2012, 01, 02).IsBeginningOfMonth());
            Assert.IsFalse(new DateTime(2012, 02, 29).IsBeginningOfMonth());
            Assert.IsFalse(new DateTime(2012, 04, 30).IsBeginningOfMonth());
            Assert.IsFalse(new DateTime(2012, 04, 15).IsBeginningOfMonth());
        }

        [TestMethod]
        public void ShouldReturnFalseAsEndOfMonthForAnyDayOfMonthOtherThanLastDay()
        {
            Assert.IsFalse(new DateTime(2012, 01, 02).IsEndOfMonth());
            Assert.IsFalse(new DateTime(2012, 02, 01).IsEndOfMonth());
            Assert.IsFalse(new DateTime(2012, 04, 29).IsEndOfMonth());
            Assert.IsFalse(new DateTime(2012, 04, 15).IsEndOfMonth());
        }
    }
}