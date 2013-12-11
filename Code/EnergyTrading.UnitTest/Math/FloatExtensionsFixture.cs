namespace EnergyTrading.UnitTest.Math
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Math;

    [TestClass]
    public class FloatExtensionsFixture
    {
        [TestMethod]
        public void RoundTripViaBits()
        {
            var expected = 1.2345f;
            var bits = expected.SingleToInt32Bits();
            var candidate = bits.Int32BitsToSingle();

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void UlpNan()
        {
            var candidate = float.NaN.Ulp();

            Assert.IsTrue(float.IsNaN(candidate));
        }

        [TestMethod]
        public void UlpPositiveInfinity()
        {
            var candidate = float.PositiveInfinity.Ulp();

            Assert.AreEqual(float.PositiveInfinity, candidate);
        }

        [TestMethod]
        public void UlpNegativeInfinity()
        {
            var candidate = float.NegativeInfinity.Ulp();

            Assert.AreEqual(float.PositiveInfinity, candidate);
        }

        [TestMethod]
        public void UlpPositiveZero()
        {
            var value = 0f;
            var candidate = value.Ulp();

            Assert.AreEqual(float.MinValue, candidate);
        }

        [TestMethod]
        public void UlpNegativeZero()
        {
            var value = -0f;
            var candidate = value.Ulp();

            Assert.AreEqual(float.MinValue, candidate);
        }
    }
}