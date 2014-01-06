namespace EnergyTrading.UnitTest.Validation
{
    using EnergyTrading.Validation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BooleanRuleFixture
    {
        [TestMethod]
        public void ValidIfSuppliedValueIsTrue()
        {
            var r = new BooleanRule();
            Assert.IsTrue(r.IsValid(true));
        }

        [TestMethod]
        public void InvalidIfSuppliedValueIsFalse()
        {
            var r = new BooleanRule();
            Assert.IsFalse(r.IsValid(false));
        }
    }
}