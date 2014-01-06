namespace EnergyTrading.UnitTest.Validation
{
    using EnergyTrading.Validation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringLengthRuleFixture
    {
        [TestMethod]
        public void ShortStringReturnsTrue()
        {
            var rule = new StringLengthRule(20);
            var candidate = rule.IsValid("valid");
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void LongStringReturnsFalse()
        {
            var rule = new StringLengthRule(2);
            var candidate = rule.IsValid("valid");
            Assert.IsFalse(candidate);
        }
    }
}