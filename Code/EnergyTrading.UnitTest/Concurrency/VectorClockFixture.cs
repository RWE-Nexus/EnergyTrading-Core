namespace EnergyTrading.UnitTest.Concurrency
{
    using System.Collections.Generic;

    using EnergyTrading.Concurrency;
    using EnergyTrading.Test;

    using NUnit.Framework;

    [TestFixture]
    public class VectorClockFixture : Fixture
    {
        [Test]
        public void IsBeforeDifferentElements()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(2);

            Assert.AreEqual(ConcurrencyComparison.Before, p.Compare(q));
        }

        [Test]
        public void IsBeforeSameElements()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(1);

            Assert.AreEqual(ConcurrencyComparison.Before, p.Compare(q));
        }

        [Test]
        public void IsAfterDifferent()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(2);

            Assert.AreEqual(ConcurrencyComparison.After, q.Compare(p));
        }

        [Test]
        public void IsAfterSameElements()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);
            q.Increment(1);

            Assert.AreEqual(ConcurrencyComparison.After, q.Compare(p));
        }

        [Test]
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

        [Test]
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

        [Test]
        public void IsConcurrentAgainstNull()
        {
            var p = new VectorClock();
            p.Increment(1);
            p.Increment(2);
            p.Increment(3);

            Assert.AreEqual(ConcurrencyComparison.Concurrent, p.Compare(null));
        }

        [Test]
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

        [Test]
        public void MaxNull()
        {
            var d1 = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, { 3, 3 }, { 4, 4 } };
            var d3 = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, { 3, 3 }, { 4, 4 } };
            var p = new VectorClock(d1);
            var expected = new VectorClock(d3);

            var candidate = p.Max(null);
            Assert.IsTrue(expected.Equals(candidate));
        }

        [Test]
        public void CheckToString()
        {
            var d1 = new Dictionary<int, int> { { 1, 1 }, { 2, 2 } };
            var p = new VectorClock(d1);

            var expected = "1[1]\r\n2[2]\r\n";
            Assert.AreEqual(expected, p.ToString());
        }

        [Test]
        public void ObjectEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);

            object c = q;

            Assert.IsTrue(p.Equals(c));
        }

        [Test]
        public void NullObjectEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            object c = null;

            Assert.IsFalse(p.Equals(c));
        }

        [Test]
        public void TypedEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            var q = new VectorClock();
            q.Increment(1);

            Assert.IsTrue(p.Equals(q));
        }

        [Test]
        public void NullTypedEquals()
        {
            var p = new VectorClock();
            p.Increment(1);

            VectorClock q = null;
            Assert.IsFalse(p.Equals(q));
        }
    }
}