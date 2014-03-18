namespace EnergyTrading.UnitTest.Extensions
{
    using System.Collections.Generic;

    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class DictionaryExtensionsFixture
    {
        [Test]
        public void MergeReturnsNullIfSourceIsNull()
        {
            var candidate = DictionaryExtensions.Merge(null, new Dictionary<string, string> { { "key", "value" } });
            Assert.IsNull(candidate);
        }

        [Test]
        public void MergeReturnsSourceIfDictToMergeIsNull()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(null);
            Assert.IsNotNull(candidate);
            Assert.AreSame(source, candidate);
        }

        [Test]
        public void DictionariesAreMerged()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key2", "value2" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
            Assert.AreEqual("value2", candidate["key2"]);
        }

        [Test]
        public void DuplicatesAreNotOverwrittenByDefault()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key", "value2" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
        }

        [Test]
        public void DuplicatesOverwriteIfSpecified()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.Merge(new Dictionary<string, string> { { "key", "value2" } }, true);
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count);
            Assert.AreEqual("value2", candidate["key"]);
        }

        [Test]
        public void MergeLeftWithoutOverwrites()
        {
            var source = new Dictionary<string, string> { { "key", "value" } };
            var candidate = source.MergeLeft(false, new Dictionary<string, string> { { "key", "value2" } }, new Dictionary<string, string> { { "newKey", "newValue" } });
            Assert.IsNotNull(candidate);
            Assert.AreEqual(2, candidate.Count);
            Assert.AreEqual("value", candidate["key"]);
            Assert.AreEqual("newValue", candidate["newKey"]);
        }

        [Test]
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