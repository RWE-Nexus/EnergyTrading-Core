namespace EnergyTrading.UnitTest.Xml.Linq
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Xml.Linq;

    [TestClass]
    public class ParseXmlBoolFixture
    {       
        [TestMethod]
        public void ParseTrue()
        {
            this.Parse("True", true);
        }

        [TestMethod]
        public void ParseTrueLower()
        {
            this.Parse("true", true);
        }

        [TestMethod]
        public void ParseYes()
        {
            this.Parse("Yes", true);
        }

        [TestMethod]
        public void ParseYesLower()
        {
            this.Parse("yes", true);
        }

        [TestMethod]
        public void ParseOne()
        {
            this.Parse("1", true);
        }

        [TestMethod]
        public void ParseFalse()
        {
            this.Parse("False", false);
        }

        [TestMethod]
        public void ParseFalseLower()
        {
            this.Parse("false", false);
        }

        [TestMethod]
        public void ParseNo()
        {
            this.Parse("No", false);
        }

        [TestMethod]
        public void ParseNoLower()
        {
            this.Parse("no", false);
        }

        [TestMethod]
        public void ParseZero()
        {
            this.Parse("0", false);
        }

        [TestMethod]
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