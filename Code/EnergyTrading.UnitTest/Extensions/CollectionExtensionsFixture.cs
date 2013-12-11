namespace EnergyTrading.UnitTest.Extensions
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Extensions;

    [TestClass]
    public class CollectionExtensionsFixture
    {
        [TestMethod]
        public void IsNullOrEmptyReturnsTrueForNull()
        {
            var candidate = CollectionExtensions.IsNullOrEmpty<int>(null);
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void IsNullOrEmptyReturnsTrueForEmptyList()
        {
            var candidate = new List<int>().IsNullOrEmpty();
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void IsNullOrEmptyReturnsFalseForNonEmptyList()
        {
            var candidate = new List<int> { 1 }.IsNullOrEmpty();
            Assert.IsFalse(candidate);
        }
    }
}