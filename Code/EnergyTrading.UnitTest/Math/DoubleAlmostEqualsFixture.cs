namespace EnergyTrading.UnitTest.Math
{
    using EnergyTrading.Math;

    using NUnit.Framework;

    [TestFixture]
    public class DoubleAlmostEqualsFixture
    {
        [Test]
        public void NanNan()
        {
            var expected = double.NaN;
            var candidate = double.NaN;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [Test]
        public void NanZero()
        {
            var expected = double.NaN;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [Test]
        public void ZeroNan()
        {
            var expected = 0d;
            var candidate = double.NaN;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [Test]
        public void PositiveInfinityZero()
        {
            var expected = double.PositiveInfinity;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);    
        }

        [Test]
        public void ZeroPositiveInfinity()
        {
            var expected = 0d;
            var candidate = double.PositiveInfinity;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);    
        }

        [Test]
        public void NegativeInfinityZero()
        {
            var expected = double.NegativeInfinity;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [Test]
        public void ZeroNegativeInfinity()
        {
            var expected = 0d;
            var candidate = double.NegativeInfinity;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [Test]
        public void ZeroZero()
        {
            var expected = -0d;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsTrue(result);
        }

        [Test]
        public void VaryInPosition11()
        {
            var expected = 1632.4587642911599d;
            var candidate = 1632.4587642911633d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsTrue(result);
        }
    }
}