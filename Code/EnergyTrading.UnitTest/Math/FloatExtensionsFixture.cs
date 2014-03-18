namespace EnergyTrading.UnitTest.Math
{
    using EnergyTrading.Math;

    using NUnit.Framework;

    [TestFixture]
    public class FloatExtensionsFixture
    {
        [Test]
        public void RoundTripViaBits()
        {
            var expected = 1.2345f;
            var bits = expected.SingleToInt32Bits();
            var candidate = bits.Int32BitsToSingle();

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void UlpNan()
        {
            var candidate = float.NaN.Ulp();

            Assert.IsTrue(float.IsNaN(candidate));
        }

        [Test]
        public void UlpPositiveInfinity()
        {
            var candidate = float.PositiveInfinity.Ulp();

            Assert.AreEqual(float.PositiveInfinity, candidate);
        }

        [Test]
        public void UlpNegativeInfinity()
        {
            var candidate = float.NegativeInfinity.Ulp();

            Assert.AreEqual(float.PositiveInfinity, candidate);
        }

        [Test]
        public void UlpPositiveZero()
        {
            var value = 0f;
            var candidate = value.Ulp();

            Assert.AreEqual(float.MinValue, candidate);
        }

        [Test]
        public void UlpNegativeZero()
        {
            var value = -0f;
            var candidate = value.Ulp();

            Assert.AreEqual(float.MinValue, candidate);
        }
    }
}