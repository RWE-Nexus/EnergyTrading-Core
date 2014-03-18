namespace EnergyTrading.UnitTest.Validation
{
    using EnergyTrading.Validation;

    using NUnit.Framework;

    [TestFixture]
    public class StringLengthRuleFixture
    {
        [Test]
        public void ShortStringReturnsTrue()
        {
            var rule = new StringLengthRule(20);
            var candidate = rule.IsValid("valid");
            Assert.IsTrue(candidate);
        }

        [Test]
        public void LongStringReturnsFalse()
        {
            var rule = new StringLengthRule(2);
            var candidate = rule.IsValid("valid");
            Assert.IsFalse(candidate);
        }
    }
}