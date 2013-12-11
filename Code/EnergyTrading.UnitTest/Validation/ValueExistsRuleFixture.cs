namespace EnergyTrading.UnitTest.Validation
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Validation;

    [TestClass]
    public class ValueExistsRuleFixture
    {
        private readonly List<string> data = new List<string> { "a", "b" };

        [TestMethod]
        public void TestFalseIfNotInSet()
        {
            var rule = new ValueExistsRule<string>(data);
            var candidate = rule.IsValid("c");
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void TestTrueIfInSet()
        {
            var rule = new ValueExistsRule<string>(data);
            var candidate = rule.IsValid("a");
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void UsesSuppliedComparer()
        {
            var rule = new ValueExistsRule<string>(data, StringComparer.InvariantCultureIgnoreCase);
            var candidate = rule.IsValid("B");
            Assert.IsTrue(candidate);
        }
    }
}