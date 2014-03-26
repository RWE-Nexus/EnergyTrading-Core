namespace EnergyTrading.UnitTest.Xml.Linq
{
    using System;

    using EnergyTrading.Xml.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ParseXmlBoolFixture
    {       
        [Test]
        public void ParseTrue()
        {
            this.Parse("True", true);
        }

        [Test]
        public void ParseTrueLower()
        {
            this.Parse("true", true);
        }

        [Test]
        public void ParseYes()
        {
            this.Parse("Yes", true);
        }

        [Test]
        public void ParseYesLower()
        {
            this.Parse("yes", true);
        }

        [Test]
        public void ParseOne()
        {
            this.Parse("1", true);
        }

        [Test]
        public void ParseFalse()
        {
            this.Parse("False", false);
        }

        [Test]
        public void ParseFalseLower()
        {
            this.Parse("false", false);
        }

        [Test]
        public void ParseNo()
        {
            this.Parse("No", false);
        }

        [Test]
        public void ParseNoLower()
        {
            this.Parse("no", false);
        }

        [Test]
        public void ParseZero()
        {
            this.Parse("0", false);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void ParseBool()
        {
            this.Parse("test", false);
        }

        private void Parse(string value, bool expected)
        {
            Assert.AreEqual(expected, value.ParseXmlBool(), "Parsing " + value + " failed");
        }
    }
}