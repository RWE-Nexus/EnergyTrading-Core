namespace EnergyTrading.UnitTest.Extensions
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Extensions;

    [TestClass]
    public class EnumExtensionsFixture
    {
        [TestMethod]
        public void ParseEnumValue()
        {
            var value = "Fred";
            var expected = TestEnum.Fred;

            var candidate = value.ToEnum<TestEnum>();
            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void ParseNullReturnsDefaultValue()
        {
            string value = null;
            var expected = TestEnum.Unknown;

            var candidate = value.ToEnum<TestEnum>();
            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void ParseUnknownValueReturnsDefaultValue()
        {
            var value = "Jim";
            var expected = TestEnum.Unknown;

            var candidate = value.ToEnum<TestEnum>();
            Assert.AreEqual(expected, candidate); 
        }

        [TestMethod]
        public void ParseExplicitDefaultValue()
        {
            var value = "Jim";
            var expected = TestEnum.John;

            var candidate = value.ToEnum<TestEnum>(TestEnum.John);
            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void ParseFlagsEnum()
        {
            var expected = FlagsEnum.A | FlagsEnum.B | FlagsEnum.C;
            var candidate = EnumExtensions.ToEnum<FlagsEnum>(true, FlagsEnum.Unknown, "A", "B", "C");
            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void ParseFlagsEnumNoFlagsAttribute()
        {
            try
            {
                EnumExtensions.ToEnum<TestEnum>(true, TestEnum.Unknown, "Bob", "Jim");

                throw new InvalidOperationException("Should have NotSupportedException");
            }
            catch (NotSupportedException)
            {                
            }
        }

        public enum TestEnum
        {
            Unknown = 0,
            Fred = 1,
            John = 2
        }

        [Flags]
        public enum FlagsEnum
        {
            Unknown = 0x0,
            A = 0x1,
            B = 0x2,
            C = 0x4
        }
    }
}
