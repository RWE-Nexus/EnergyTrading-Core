namespace EnergyTrading.UnitTest.Extensions
{
    using System.Collections.Generic;

    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class CollectionExtensionsFixture
    {
        [Test]
        public void IsNullOrEmptyReturnsTrueForNull()
        {
            var candidate = CollectionExtensions.IsNullOrEmpty<int>(null);
            Assert.IsTrue(candidate);
        }

        [Test]
        public void IsNullOrEmptyReturnsTrueForEmptyList()
        {
            var candidate = new List<int>().IsNullOrEmpty();
            Assert.IsTrue(candidate);
        }

        [Test]
        public void IsNullOrEmptyReturnsFalseForNonEmptyList()
        {
            var candidate = new List<int> { 1 }.IsNullOrEmpty();
            Assert.IsFalse(candidate);
        }
    }
}