namespace EnergyTrading.UnitTest.Extensions
{
    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class StringExtensionsFixture
    {
        [Test]
        public void SourceIsUnaffectedIfNotNullOrEmpty()
        {
            var src = "input";
            src = src.DefaultIfNullOrEmpty("value");
            Assert.AreEqual("input", src);
        }

        [Test]
        public void ValueIsAssignedIfSourceIsNull()
        {
            string src = null;
            src = src.DefaultIfNullOrEmpty("value");
            Assert.AreEqual("value", src);
        }

        [Test]
        public void ValueIsAssignedIfSourceIsEmpty()
        {
            var src = string.Empty;
            src = src.DefaultIfNullOrEmpty("value");
            Assert.AreEqual("value", src);
        }

        [Test]
        public void ValueIsNotAssignedIfSourceOnlyContainsWhiteSpace()
        {
            var src = " ";
            src = src.DefaultIfNullOrEmpty("value");
            Assert.AreEqual(" ", src);
        }

        [Test]
        public void AppendToEndIfTailNotPresent()
        {
            var tail = "\\";
            var src = "test";
            var expected = src + tail;
            var candidate = src.AppendValueToEndIfMissing(tail);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void DontAppendToEndIfTailPresent()
        {
            var tail = "\\";
            var src = "test" + tail;
            var expected = src;
            var candidate = src.AppendValueToEndIfMissing(tail);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void DontAppendToEndIfEmpty()
        {
            var tail = "\\";
            var src = string.Empty;
            var expected = string.Empty;
            var candidate = src.AppendValueToEndIfMissing(tail);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void DontAppendToEndIfNull()
        {
            var tail = "\\";
            string src = null;
            string expected = null;
            var candidate = src.AppendValueToEndIfMissing(tail);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void DontAppendToEndIfWhitespace()
        {
            var tail = "\\";
            var src = " ";
            var expected = " ";
            var candidate = src.AppendValueToEndIfMissing(tail);

            Assert.AreEqual(expected, candidate);
        }
    }
}
