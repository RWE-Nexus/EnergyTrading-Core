namespace EnergyTrading.UnitTest.Concurrency
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Concurrency;
    using EnergyTrading.Test;

    [TestClass]
    public class VectorClockFixture : Fixture
    {
        [TestMethod]
        public void IsBeforeDifferentElements()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(2);

            Assert.AreEqual(ConcurrencyComparison.Before, p.Compare(q));
        }

        [TestMethod]
        public void IsBeforeSameElements()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(1);

            Assert.AreEqual(ConcurrencyComparison.Before, p.Compare(q));
        }

        [TestMethod]
        public void IsAfterDifferent()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(2);

            Assert.AreEqual(ConcurrencyComparison.After, q.Compare(p));
        }

        [TestMethod]
        public void IsAfterSameElements()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(1);

            Assert.AreEqual(ConcurrencyComparison.After, q.Compare(p));
        }

        [TestMethod]
        public void IsEqual()
        {
            var p = new VectorClock();
            p.Increment(1);
            p.Increment(2);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(2);

            Assert.AreEqual(ConcurrencyComparison.Equal, p.Compare(q));
        }

        [TestMethod]
        public void IsConcurrent()
        {
            var p = new VectorClock();
            p.Increment(1);
            p.Increment(2);
            p.Increment(3);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(3);
            q.Increment(4);

            Assert.AreEqual(ConcurrencyComparison.Concurrent, p.Compare(q));   
        }

        [TestMethod]
        public void IsConcurrentAgainstNull()
        {
            var p = new VectorClock();
            p.Increment(1);
            p.Increment(2);
            p.Increment(3);

            Assert.AreEqual(ConcurrencyComparison.Concurrent, p.Compare(null));
        }

        [TestMethod]
        public void Max()
        {
            var d1 = new Dictionary<int, int> { { 1, 1 }, {2, 1}, {3, 3}, {4, 4} };
            var d2 = new Dictionary<int, int> { { 1, 1 }, {2, 2}, {3, 1}, {5, 5} };
            var d3 = new Dictionary<int, int> { { 1, 1 }, { 2, 2 }, { 4, 4 }, { 3, 3 }, { 5, 5 } };
            var p = new VectorClock(d1);
            var q = new VectorClock(d2);
            var expected = new VectorClock(d3);

            var candidate = p.Max(q);
            Assert.IsTrue(expected.Equals(candidate));
        }

        [TestMethod]
        public void MaxNull()
        {
            var d1 = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, { 3, 3 }, { 4, 4 } };
            var d3 = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, { 3, 3 }, { 4, 4 } };
            var p = new VectorClock(d1);
            var expected = new VectorClock(d3);

            var candidate = p.Max(null);
            Assert.IsTrue(expected.Equals(candidate));
        }

        [TestMethod]
        public void CheckToString()
        {
            var d1 = new Dictionary<int, int> { { 1, 1 }, { 2, 2 } };
            var p = new VectorClock(d1);

            var expected = "1[1]\r\n2[2]\r\n";
            Assert.AreEqual(expected, p.ToString());
        }

        [TestMethod]
        public void ObjectEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);

            object c = q;

            Assert.IsTrue(p.Equals(c));
        }

        [TestMethod]
        public void NullObjectEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            object c = null;

            Assert.IsFalse(p.Equals(c));
        }

        [TestMethod]
        public void TypedEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);

            Assert.IsTrue(p.Equals(q));
        }

        [TestMethod]
        public void NullTypedEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            VectorClock q = null;
            Assert.IsFalse(p.Equals(q));
        }
    }
}