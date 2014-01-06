namespace EnergyTrading.UnitTest.Xml
{
    using System.Reflection;
    using System.Xml.Linq;

    using EnergyTrading.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SimpleSchemaValidatorFixture
    {
        [TestMethod]
        public void CorrectXmlIsValid()
        {
            using (var xmlStream = Assembly.GetAssembly(typeof(SimpleSchemaValidatorFixture)).GetManifestResourceStream("EnergyTrading.UnitTest.Xml.ValidXml.xml"))
            {
                var element = XElement.Load(xmlStream);
                using (var schemaStream = Assembly.GetAssembly(typeof(SimpleSchemaValidatorFixture)).GetManifestResourceStream("EnergyTrading.UnitTest.Xml.TestSchema.xsd"))
                {
                    var candidate = SimpleSchemaValidator.TryValidate(schemaStream, element, "http://rwe.com/schema/simple/test/1");
                    Assert.IsTrue(candidate);
                }
            }
        }

        [TestMethod]
        public void InvalidXmlIsNotValid()
        {
            using (var xmlStream = Assembly.GetAssembly(typeof(SimpleSchemaValidatorFixture)).GetManifestResourceStream("EnergyTrading.UnitTest.Xml.InvalidXml.xml"))
            {
                var element = XElement.Load(xmlStream);
                using (var schemaStream = Assembly.GetAssembly(typeof(SimpleSchemaValidatorFixture)).GetManifestResourceStream("EnergyTrading.UnitTest.Xml.TestSchema.xsd"))
                {
                    var candidate = SimpleSchemaValidator.TryValidate(schemaStream, element, "http://rwe.com/schema/simple/test/1");
                    Assert.IsFalse(candidate);
                }
            }
        }
    }
}