namespace EnergyTrading.UnitTest.Validation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Validation;

    [TestClass]
    public class StringNotNullOrEmptyRuleFixture
    {
        [TestMethod]
        public void ValidStringReturnsTrue()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid("valid");
            Assert.IsTrue(candidate);
            Assert.IsTrue(string.IsNullOrEmpty(rule.Message));
        }

        [TestMethod]
        public void NullStringReturnsFalse()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid(null);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or whitespace", rule.Message);
        }

        [TestMethod]
        public void EmptyStringReturnsFalse()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid(null);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or whitespace", rule.Message);
        }

        [TestMethod]
        public void ByDefaultWhiteSpaceIsNotAllowed()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid("   ");
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or whitespace", rule.Message);
        }

        [TestMethod]
        public void AllowWhiteSpaceWithNullIsStillFalse()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid(null);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or empty", rule.Message);
        }

        [TestMethod]
        public void AllowWhiteSpaceWithEmptyIsStillFalse()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid(string.Empty);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or empty", rule.Message);
        }

        [TestMethod]
        public void ValidIsStillValidWithWhiteSpace()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid("valid");
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void WhiteSpaceIsAllowedCorrectly()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid("   ");
            Assert.IsTrue(candidate);
        }
    }
}