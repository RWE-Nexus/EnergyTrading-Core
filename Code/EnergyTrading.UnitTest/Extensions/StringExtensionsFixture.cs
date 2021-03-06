﻿namespace EnergyTrading.UnitTest.Extensions
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

        [Test]
        [TestCase("code", false, null, TestName = "null array")]
        [TestCase("code", false, new string[] {}, TestName = "empty array")]
        [TestCase("code", false, new[] { "codes" }, TestName = "ExactMatchIsWholeValue")]
        [TestCase("code", true, new[] { "code" }, TestName = "ExactMatch")]
        [TestCase("code", true, new[] { "CODE" }, TestName = "ExactMatchCaseIsInsensitive")]
        [TestCase("codeIsPrefix", true, new[] { "CODE*" }, TestName = "StartsWith")]
        [TestCase("codeIsPrefix", true, new[] { "CODE.*" }, TestName = "StartsWithRegexNotation")]
        [TestCase("SuffixIsCode", true, new[] { "*CODE" }, TestName = "EndsWith")]
        [TestCase("SuffixIsCode", true, new[] { "*.CODE" }, TestName = "EndsWithRegexNotation")]
        [TestCase("MatchAnything", true, new[] { "*" }, TestName = "MatchAnythingAsterixOnly")]
        [TestCase("MatchAnything", true, new[] { "*.*" }, TestName = "MatchAnythingAsterixDotAsterix")]
        [TestCase("Prefix.Code.Suffix", true, new[] { "*CODE*" }, TestName = "Contains")]
        [TestCase("Prefix.Code.Suffix", true, new[] { "*.CODE.*" }, TestName = "ContainsRegexNotationIsh")]
        [TestCase("Prefix.Code.Suffix", true, new[] { "*CODE.*" }, TestName = "ContainsMixedNotation1")]
        [TestCase("Prefix.Code.Suffix", true, new[] { "*.CODE*" }, TestName = "ContainsMixedNotation2")]
        [TestCase("Prefix.Code.Suffix", true, new[] { "falseValue", "*CODE*" }, TestName = "ChecksThroughArrayForMatch")]
        [TestCase("Prefix.Code.Suffix", false, new[] { "falseValue", "*CODE" }, TestName = "NoMatchInArray")]
        [TestCase(null, false, new[] { "falseValue", "*CODE" }, TestName = "FalseForNullCode")]
        [TestCase("", false, new[] { "falseValue", "*CODE" }, TestName = "FalseForEmptyCode")]
        [TestCase("   ", false, new[] { "falseValue", "*CODE" }, TestName = "FalseForWhitespaceCode")]
        [TestCase("Nexus.Counterparty.VerificationTest.Result", true, new[] { "Nexus.Counterparty.Error.*", "Nexus.Counterparty.VerificationTest.Result" }, TestName = "CounterPartyExample")]
        [TestCase("F1PQ12345", false, new[] { "F1PY*" }, TestName = "TradeBugFixFalse")]
        [TestCase("F1PQ12345", true, new[] { "F1PQ*" }, TestName = "TradeBugFixTrue")]
        public void ShouldHandleCases(string code, bool expectedResult, string[] handlerCodes)
        {
            Assert.That(handlerCodes.Matches(code), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("code", false, -1, null, TestName = "null array")]
        [TestCase("code", false, -1, new string[] { }, TestName = "empty array")]
        [TestCase("code", false, -1, new[] { "codes" }, TestName = "ExactMatchIsWholeValue")]
        [TestCase("code", true, 4, new[] { "code" }, TestName = "ExactMatch")]
        [TestCase("code", true, 4, new[] { "CODE" }, TestName = "ExactMatchCaseIsInsensitive")]
        [TestCase("codeIsPrefix", true, 4, new[] { "CODE*" }, TestName = "StartsWith")]
        [TestCase("codeIsPrefix", true, 4, new[] { "CODE.*" }, TestName = "StartsWithRegexNotation")]
        [TestCase("SuffixIsCode", true, 4, new[] { "*CODE" }, TestName = "EndsWith")]
        [TestCase("SuffixIsCode", true, 4, new[] { "*.CODE" }, TestName = "EndsWithRegexNotation")]
        [TestCase("MatchAnything", true, 0, new[] { "*" }, TestName = "MatchAnythingAsterixOnly")]
        [TestCase("MatchAnything", true, 0, new[] { "*.*" }, TestName = "MatchAnythingAsterixDotAsterix")]
        [TestCase("Prefix.Code.Suffix", true, 4, new[] { "*CODE*" }, TestName = "Contains")]
        [TestCase("Prefix.Code.Suffix", true, 4, new[] { "*.CODE.*" }, TestName = "ContainsRegexNotationIsh")]
        [TestCase("Prefix.Code.Suffix", true, 4, new[] { "*CODE.*" }, TestName = "ContainsMixedNotation1")]
        [TestCase("Prefix.Code.Suffix", true, 4, new[] { "*.CODE*" }, TestName = "ContainsMixedNotation2")]
        [TestCase("Prefix.Code.Suffix", true, 4, new[] { "falseValue", "*CODE*" }, TestName = "ChecksThroughArrayForMatch")]
        [TestCase("Prefix.Code.Suffix", false, -1, new[] { "falseValue", "*CODE" }, TestName = "NoMatchInArray")]
        [TestCase(null, false, -1, new[] { "falseValue", "*CODE" }, TestName = "FalseForNullCode")]
        [TestCase("", false, -1, new[] { "falseValue", "*CODE" }, TestName = "FalseForEmptyCode")]
        [TestCase("   ", false, -1, new[] { "falseValue", "*CODE" }, TestName = "FalseForWhitespaceCode")]
        [TestCase("Nexus.Counterparty.VerificationTest.Result", true, 42, new[] { "Nexus.Counterparty.Error.*", "Nexus.Counterparty.VerificationTest.Result" }, TestName = "CounterPartyExample")]
        [TestCase("F1PQ12345", false, -1, new[] { "F1PY*" }, TestName = "TradeBugFixFalse")]
        [TestCase("F1PQ12345", true, 4, new[] { "F1PQ*" }, TestName = "TradeBugFixTrue")]
        public void ShouldHandleCasesWithOutputParameter(string code, bool expectedResult, int expectedMatchingCharacters, string[] handlerCodes)
        {
            int matchedChars;
            Assert.That(handlerCodes.Matches(code, out matchedChars), Is.EqualTo(expectedResult));
            Assert.That(matchedChars, Is.EqualTo(expectedMatchingCharacters));
        }
    }
}
