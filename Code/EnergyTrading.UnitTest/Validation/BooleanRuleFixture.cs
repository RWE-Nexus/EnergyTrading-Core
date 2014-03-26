namespace EnergyTrading.UnitTest.Validation
{
    using EnergyTrading.Validation;

    using NUnit.Framework;

    [TestFixture]
    public class BooleanRuleFixture
    {
        [Test]
        public void ValidIfSuppliedValueIsTrue()
        {
            var r = new BooleanRule();
            Assert.IsTrue(r.IsValid(true));
        }

        [Test]
        public void InvalidIfSuppliedValueIsFalse()
        {
            var r = new BooleanRule();
            Assert.IsFalse(r.IsValid(false));
        }
    }
}