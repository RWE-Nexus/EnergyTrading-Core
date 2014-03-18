namespace EnergyTrading.UnitTest.Validation
{
    using EnergyTrading.Validation;

    using NUnit.Framework;

    [TestFixture]
    public class StringNotNullOrEmptyRuleFixture
    {
        [Test]
        public void ValidStringReturnsTrue()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid("valid");
            Assert.IsTrue(candidate);
            Assert.IsTrue(string.IsNullOrEmpty(rule.Message));
        }

        [Test]
        public void NullStringReturnsFalse()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid(null);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or whitespace", rule.Message);
        }

        [Test]
        public void EmptyStringReturnsFalse()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid(null);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or whitespace", rule.Message);
        }

        [Test]
        public void ByDefaultWhiteSpaceIsNotAllowed()
        {
            var rule = new StringNotNullOrEmptyRule();
            var candidate = rule.IsValid("   ");
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or whitespace", rule.Message);
        }

        [Test]
        public void AllowWhiteSpaceWithNullIsStillFalse()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid(null);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or empty", rule.Message);
        }

        [Test]
        public void AllowWhiteSpaceWithEmptyIsStillFalse()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid(string.Empty);
            Assert.IsFalse(candidate);
            Assert.AreEqual("value is null or empty", rule.Message);
        }

        [Test]
        public void ValidIsStillValidWithWhiteSpace()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid("valid");
            Assert.IsTrue(candidate);
        }

        [Test]
        public void WhiteSpaceIsAllowedCorrectly()
        {
            var rule = new StringNotNullOrEmptyRule { AllowWhiteSpace = true };
            var candidate = rule.IsValid("   ");
            Assert.IsTrue(candidate);
        }
    }
}