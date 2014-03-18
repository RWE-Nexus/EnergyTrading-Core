namespace EnergyTrading.UnitTest.Math
{
    using EnergyTrading.Math;

    using NUnit.Framework;

    [TestFixture]
    public class DoubleExtensionsFixture
    {
        [Test]
        public void UlpNan()
        {
            var candidate = double.NaN.Ulp();

            Assert.IsTrue(double.IsNaN(candidate));
        }

        [Test]
        public void UlpPositiveInfinity()
        {
            var candidate = double.PositiveInfinity.Ulp();

            Assert.AreEqual(double.PositiveInfinity, candidate);
        }

        [Test]
        public void UlpNegativeInfinity()
        {
            var candidate = double.NegativeInfinity.Ulp();

            Assert.AreEqual(double.PositiveInfinity, candidate);            
        }

        [Test]
        public void UlpPositiveZero()
        {
            var value = 0d;
            var candidate = value.Ulp();

            Assert.AreEqual(double.MinValue, candidate);     
        }

        [Test]
        public void UlpNegativeZero()
        {
            var value = -0d;
            var candidate = value.Ulp();

            Assert.AreEqual(double.MinValue, candidate);
        }
    }
}