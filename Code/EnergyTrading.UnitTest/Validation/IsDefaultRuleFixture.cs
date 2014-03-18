namespace EnergyTrading.UnitTest.Validation
{
    using System;

    using EnergyTrading.Validation;

    using NUnit.Framework;

    [TestFixture]
    public class IsDefaultRuleFixture
    {
        [Test]
        public void IsDefaultValueType()
        {
            var r = new IsDefaultRule<DateTime>();
            Assert.IsTrue(r.IsValid(default(DateTime)));
            Assert.IsTrue(r.IsValid(DateTime.MinValue));
        }

        [Test]
        public void IsNotDefaultValueType()
        {
            var r = new IsDefaultRule<DateTime>();
            Assert.IsFalse(r.IsValid(new DateTime(2013, 12, 12)));
            Assert.IsFalse(r.IsValid(DateTime.Now));
        }

        [Test]
        public void IsDefaultReferenceType()
        {
            var r = new IsDefaultRule<string>();
            Assert.IsTrue(r.IsValid(default(string)));
            Assert.IsTrue(r.IsValid(null));
        }

        [Test]
        public void IsNotDefaultReferenceType()
        {
            var r = new IsDefaultRule<string>();
            Assert.IsFalse(r.IsValid(string.Empty));
            Assert.IsFalse(r.IsValid("some test string"));
        }
    }
}