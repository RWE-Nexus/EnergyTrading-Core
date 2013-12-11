namespace EnergyTrading.UnitTest.Math
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Math;

    [TestClass]
    public class DoubleExtensionsFixture
    {
        [TestMethod]
        public void UlpNan()
        {
            var candidate = double.NaN.Ulp();

            Assert.IsTrue(double.IsNaN(candidate));
        }

        [TestMethod]
        public void UlpPositiveInfinity()
        {
            var candidate = double.PositiveInfinity.Ulp();

            Assert.AreEqual(double.PositiveInfinity, candidate);
        }

        [TestMethod]
        public void UlpNegativeInfinity()
        {
            var candidate = double.NegativeInfinity.Ulp();

            Assert.AreEqual(double.PositiveInfinity, candidate);            
        }

        [TestMethod]
        public void UlpPositiveZero()
        {
            var value = 0d;
            var candidate = value.Ulp();

            Assert.AreEqual(double.MinValue, candidate);     
        }

        [TestMethod]
        public void UlpNegativeZero()
        {
            var value = -0d;
            var candidate = value.Ulp();

            Assert.AreEqual(double.MinValue, candidate);
        }
    }
}