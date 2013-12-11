namespace EnergyTrading.UnitTest.Math
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Math;

    [TestClass]
    public class DoubleAlmostEqualsFixture
    {
        [TestMethod]
        public void NanNan()
        {
            var expected = double.NaN;
            var candidate = double.NaN;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NanZero()
        {
            var expected = double.NaN;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ZeroNan()
        {
            var expected = 0d;
            var candidate = double.NaN;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PositiveInfinityZero()
        {
            var expected = double.PositiveInfinity;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);    
        }

        [TestMethod]
        public void ZeroPositiveInfinity()
        {
            var expected = 0d;
            var candidate = double.PositiveInfinity;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);    
        }

        [TestMethod]
        public void NegativeInfinityZero()
        {
            var expected = double.NegativeInfinity;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ZeroNegativeInfinity()
        {
            var expected = 0d;
            var candidate = double.NegativeInfinity;

            var result = expected.AlmostEquals(candidate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ZeroZero()
        {
            var expected = -0d;
            var candidate = 0d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VaryInPosition11()
        {
            var expected = 1632.4587642911599d;
            var candidate = 1632.4587642911633d;

            var result = expected.AlmostEquals(candidate);
            Assert.IsTrue(result);
        }
    }
}