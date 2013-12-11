namespace EnergyTrading.UnitTest.Xml.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.XPath;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Logging;
    using EnergyTrading.Xml;
    using EnergyTrading.Xml.Linq;

    [DeploymentItem("Mapping\\Trade.xml", "Mapping")]
    [TestClass]
    public class XmlLinqExtensionsFixture
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string XmlPath = ".\\Mapping\\Trade.xml";

        private const string Xml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob>b</Bob>
                                     </Fred>";

        [TestMethod]
        public void XDocumentNamespaceFile()
        {
            string xml;
            using (var reader = new StreamReader(XmlPath))
            {
                xml = reader.ReadToEnd();
                reader.Close();
            }
            var document = XDocument.Parse(xml);

            this.TimeIt(document.Namespaces().ToList);
        }

        [TestMethod]
        public void XDocumentNamespaceXml()
        {
            var document = XDocument.Parse(Xml);

            this.TimeIt(document.Namespaces().ToList);
        }

        [TestMethod]
        public void XPathDocumentNamespaceFile()
        {
            var document = new XPathDocument(XmlPath);

            this.TimeIt(document.Namespaces().ToList);
        }

        [TestMethod]
        public void XPathDocumentNamespaceXml()
        {
            var document = new XPathDocument(new StringReader(Xml));

            this.TimeIt(document.Namespaces().ToList);
        }

        [TestMethod]
        public void XPathDocumenNamespace2File()
        {
            var document = new XPathDocument(XmlPath);

            this.TimeIt(document.Namespaces2().ToList);
        }

        [TestMethod]
        public void XPathDocumenNamespace2Xml()
        {
            var document = new XPathDocument(new StringReader(Xml));

            this.TimeIt(document.Namespaces2().ToList);
        }

        private void TimeIt(Func<List<Tuple<string, string>>> f)
        {
            var w = new Stopwatch();
            w.Start();

            var d = f();
            for (var i = 0; i < 1000; i++)
            {
                d = f();
            }

            w.Stop();

            Logger.InfoFormat("Found {0} in {1} ms", d.Count, w.ElapsedMilliseconds);
        }

        [TestMethod]
        public void ShouldNormalizeWithoutSchema()
        {
            var input = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                                <Child>1</Child>
                                            </Root>");

            var afterNormalize = input.Normalize(null);

            Assert.IsNotNull(afterNormalize);
        }

        [TestMethod]
        public void ShouldNotChangeTheContentOfNormalizeXml()
        {
            var input = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                                <Child>1</Child>
                                            </Root>");

            var afterNormalize = input.Normalize(null);

            Assert.IsNotNull(afterNormalize);
            Assert.IsTrue(afterNormalize.DeepEqualsWithNormalization(input, null));
        }

        [TestMethod]
        public void ShouldReturnTrueForTwoSemanticallyEquivalentOrIsometricXmls()
        {
            var doc1 = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                            <Child>1</Child>
                                        </Root>");

            var doc2 = XDocument.Parse(@"<n:Root xmlns:n='http://www.northwind.com'>
                                            <n:Child>1</n:Child>
                                        </n:Root>");

            Assert.IsTrue(doc1.DeepEqualsWithNormalization(doc2, null));
        }

        [TestMethod]
        public void ShouldReturnTrueIfTwoSemanticallyEquivalentOrIsometricXmlsHasDifferentOrderOfAttributes()
        {
            var doc1 = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                            <Child a='1' b='2'>1</Child>
                                        </Root>");

            var doc2 = XDocument.Parse(@"<n:Root xmlns:n='http://www.northwind.com'>
                                            <n:Child  b='2' a='1'>1</n:Child>
                                        </n:Root>");

            Assert.IsTrue(doc1.DeepEqualsWithNormalization(doc2, null));
        }

        [TestMethod]
        public void ShouldNormalizeWithSchema()
        {
            var doc = XDocument.Parse(@"<Root xmlns:n='http://www.northwind.com'>
                                            <Child1>abc</Child1>
                                            <Child2>xyz</Child2>
                                        </Root>");

            var afterNormaize = doc.Normalize(this.SchemaSet);
            Assert.IsNotNull(afterNormaize);
        }

        [TestMethod]
        public void ShouldReturnTrueForTwoSemanticallyEquivalentOrIsometricXmlsWithSchemaValidation()
        {
            var doc1 = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                            <Child1>1</Child1>
                                            <Child2>1</Child2>
                                        </Root>");

            var doc2 = XDocument.Parse(@"<n:Root xmlns:n='http://www.northwind.com'>
                                            <n:Child1>1</n:Child1>
                                            <n:Child2>1</n:Child2>
                                        </n:Root>");


            Assert.IsTrue(doc1.DeepEqualsWithNormalization(doc2, SchemaSet));
        }

        [TestMethod]
        public void ToXElementBool()
        {
            var value = true;

            var candidate = value.ToXElement("Test");

            Assert.AreEqual("true", candidate.Value);
        }

        [TestMethod]
        public void ToXElementBoolDefaultValue()
        {
            var value = false;

            var candidate = value.ToXElement("Test");

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void ToXElementBoolOutputDefaultValue()
        {
            var value = false;

            var candidate = value.ToXElement("Test", outputDefault: true);

            Assert.AreEqual("false", candidate.Value); 
        }

        [TestMethod]
        public void ToXElementInt()
        {
            var value = 1;

            var candidate = value.ToXElement("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [TestMethod]
        public void ToXElementIntDefaultValue()
        {
            var value = 0;

            var candidate = value.ToXElement("Test");

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void ToXElementIntOutputDefaultValue()
        {
            var value = 3;

            var candidate = value.ToXElement("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [TestMethod]
        public void ToXElementDecimal()
        {
            decimal value = 1;

            var candidate = value.ToXElement("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [TestMethod]
        public void ToXElementDecimaltDefaultValue()
        {
            decimal value = 0;

            var candidate = value.ToXElement("Test");

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void ToXElementDecimalOutputDefaultValue()
        {
            decimal value = 3;

            var candidate = value.ToXElement("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [TestMethod]
        public void ToXElementDateTime()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXElement("Test");
            Assert.AreEqual("2012-05-13T23:14:34Z", candidate.Value);
        }

        [TestMethod]
        public void ToXElementDateTimeWithFormat()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXElement("Test", format: XmlExtensions.DateFormat);
            Assert.AreEqual("2012-05-13", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeBool()
        {
            var value = true;

            var candidate = value.ToXAttribute("Test");

            Assert.AreEqual("true", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeBoolDefaultValue()
        {
            var value = false;

            var candidate = value.ToXAttribute("Test");

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void ToXAttributeBoolOutputDefaultValue()
        {
            var value = false;

            var candidate = value.ToXAttribute("Test", outputDefault: true);

            Assert.AreEqual("false", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeInt()
        {
            var value = 1;

            var candidate = value.ToXAttribute("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeIntDefaultValue()
        {
            var value = 0;

            var candidate = value.ToXAttribute("Test");

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void ToXAttributeIntOutputDefaultValue()
        {
            var value = 3;

            var candidate = value.ToXAttribute("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeDecimal()
        {
            decimal value = 1;

            var candidate = value.ToXAttribute("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeDecimaltDefaultValue()
        {
            decimal value = 0;

            var candidate = value.ToXAttribute("Test");

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void ToXAttributeDecimalOutputDefaultValue()
        {
            decimal value = 3;

            var candidate = value.ToXAttribute("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeDateTime()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXAttribute("Test");
            Assert.AreEqual("2012-05-13T23:14:34Z", candidate.Value);
        }

        [TestMethod]
        public void ToXAttributeDateTimeWithFormat()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXAttribute("Test", format: XmlExtensions.DateFormat);
            Assert.AreEqual("2012-05-13", candidate.Value);
        }

        [TestMethod]
        public void GetChildElementValueReturnsNullForNullElement()
        {
            var candidate = XmlLinqExtensions.GetChildElementValue(null, "test");
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void GetChildElementValueReturnsNullIfNoChildElements()
        {
            var candidate = new XElement("element").GetChildElementValue("test");
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void GetChildElementValueReturnsValueIfPresent()
        {
            var candidate = new XElement("element", new XElement("test", "value")).GetChildElementValue("test");
            Assert.IsNotNull(candidate);
            Assert.AreEqual("value", candidate);
        }

        private XmlSchemaSet SchemaSet
        {
            get
            {
                var xsdMarkup =
                    @"<xsd:schema attributeFormDefault='unqualified' elementFormDefault='qualified' version='1.0' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                      <xsd:element name='Root'>
                        <xsd:complexType>
                          <xsd:sequence>
                            <xsd:element name='Child1' type='xsd:string' />
                            <xsd:element name='Child2' type='xsd:string' />
                          </xsd:sequence>
                        </xsd:complexType>
                      </xsd:element>
                    </xsd:schema>";
                var schemas = new XmlSchemaSet();
                schemas.Add(@"http://www.northwind.com", XmlReader.Create(new StringReader(xsdMarkup)));

                return schemas;
            }
        }
    }
}