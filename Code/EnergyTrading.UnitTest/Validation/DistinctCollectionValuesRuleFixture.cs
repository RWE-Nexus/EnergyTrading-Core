namespace EnergyTrading.UnitTest.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Configuration;
    using EnergyTrading.Validation;

    [TestClass]
    public class DistinctCollectionValuesRuleFixture
    {
        private class TestItem
        {
            public string Value { get; set; }
        }

        private IEnumerable<TestItem> CreateItems(IEnumerable<string> values)
        {
            return values.Select(x => new TestItem { Value = x }).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InvalidContruction()
        {
            new DistinctCollectionValuesRule<TestItem, string>(null, null);
        }

        [TestMethod]
        public void Construction()
        {
            var candidate = new DistinctCollectionValuesRule<TestItem, string>(x => x.Value, null);
            Assert.IsNotNull(candidate);
        }

        [TestMethod]
        public void RuleIsValidForNullList()
        {
            var candidate = new DistinctCollectionValuesRule<TestItem, string>(x => x.Value, null);
            Assert.IsTrue(candidate.IsValid(null));
        }

        [TestMethod]
        public void RuleIsValidForEmptyList()
        {
            var candidate = new DistinctCollectionValuesRule<TestItem, string>(x => x.Value, null);
            Assert.IsTrue(candidate.IsValid(new List<TestItem>()));
        }

        [TestMethod]
        public void RuleIsValidForDistinctList()
        {
            var candidate = new DistinctCollectionValuesRule<TestItem, string>(x => x.Value, null);
            Assert.IsTrue(candidate.IsValid(CreateItems(new[] { "John", "Paul", "George", "Ringo" })));
        }

        [TestMethod]
        public void RuleIsNotValidForDuplicateList()
        {
            var candidate = new DistinctCollectionValuesRule<TestItem, string>(x => x.Value, null);
            Assert.IsFalse(candidate.IsValid(CreateItems(new[] { "John", "Paul", "George", "Ringo", "John" })));
        }

        [TestMethod]
        public void RuleUsesSuppliedComparer()
        {
            var candidate = new DistinctCollectionValuesRule<TestItem, string>(x => x.Value, StringComparer.InvariantCultureIgnoreCase);
            Assert.IsFalse(candidate.IsValid(CreateItems(new[] { "John", "Paul", "George", "Ringo", "john" })));
            Assert.AreEqual(candidate.Message, "The list of items contains duplicate values");
        }

        [TestMethod]
        public void RuleWorksWithOurConfigurationCollections()
        {
            var candidate = new DistinctCollectionValuesRule<NamedConfigElement, string>(x => x.Name, StringComparer.InvariantCultureIgnoreCase);
            Assert.IsFalse(candidate.IsValid(new NamedConfigElementCollection<NamedConfigElement> { new NamedConfigElement { Name = "John"}, new NamedConfigElement { Name = "john"} }));
            Assert.AreEqual(candidate.Message, "The list of items contains duplicate values");
        }
    }
}