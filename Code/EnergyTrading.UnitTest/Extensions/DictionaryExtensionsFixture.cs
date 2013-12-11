namespace EnergyTrading.UnitTest.Extensions
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Extensions;

    [TestClass]
    public class DictionaryExtensionsFixture
    {
        [TestMethod]
        public void MergeReturnsNullIfSourceIsNull()
        {
            var candidate = DictionaryExtensions.Merge(null, new Dictionary<string, string> { { "key", "value" } });
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void MergeReturnsSourceIfDictToMergeIsNull()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(null);
            Assert.IsNotNull(candidate);
            Assert.AreSame(source, candidate);
        }

        [TestMethod]
        public void DictionariesAreMerged()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key2", "value2" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
            Assert.AreEqual("value2", candidate["key2"]);
        }

        [TestMethod]
        public void DuplicatesAreNotOverwrittenByDefault()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key", "value2" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
        }

        [TestMethod]
        public void DuplicatesOverwriteIfSpecified()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key", "value2" } }, true);
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count);
            Assert.AreEqual("value2", candidate["key"]);
        }

        [TestMethod]
        public void MergeLeftWithoutOverwrites()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.MergeLeft(false, new Dictionary<string, string> { { "key", "value2" } }, new Dictionary<string, string> { { "newKey", "newValue" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
            Assert.AreEqual("newValue", candidate["newKey"]);
        }

        [TestMethod]
        public void MergeLeftWithOverwrites()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.MergeLeft(true, new Dictionary<string, string> { { "key", "value2" } }, new Dictionary<string, string> { { "newKey", "newValue" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value2", candidate["key"]);
            Assert.AreEqual("newValue", candidate["newKey"]);
        }
    }
}