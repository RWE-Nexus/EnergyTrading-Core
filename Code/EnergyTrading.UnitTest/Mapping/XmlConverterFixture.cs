namespace EnergyTrading.UnitTest.Mapping
{
    using System;
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Xml.Serialization;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class XmlConverterFixture : Fixture
    {
        [Test]
        public void MustProvideVersionDetector()
        {
            var factory = new Mock<IXmlMappingEngineFactory>();

            try
            {
                new XmlConverter(null, factory.Object);

                throw new Exception("Expected ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("versionDetector", ex.ParamName, "Parameters differ");
            }
        }

        [Test]
        public void MustProvideMappingEngineFactory()
        {
            var detector = new Mock<IXmlVersionDetector>();

            try
            {
                new XmlConverter(detector.Object, null);

                throw new Exception("Expected ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("mappingEngineFactory", ex.ParamName, "Parameters differ");
            }
        }

        [Test]
        public void FromXmlNullOnNullXml()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            string xml = null;

            // Act
            var candidate = converter.FromXml<Content>(xml);

            // Assert
            Assert.IsNull(candidate);
        }

        [Test]
        public void FromXmlErrorOnInvalidXml()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            // Act
            try
            {
                converter.FromXml<Content>("<Test>");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException)
            {
                // Don't bother checking actual error message - too fragile
            }
        }

        [Test]
        public void FromXmlNullOnNullXElement()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            XElement element = null;

            // Act
            var candidate = converter.FromXml<Content>(element);

            // Assert
            Assert.IsNull(candidate);
        }

        [Test]
        public void FromXmlErrorOnVersionDetectionFailure()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);
            
            detector.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(string.Empty);

            // Act
            try
            {
                converter.FromXml<Content>("<Test></Test>");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException)
            {
                // Don't bother checking actual error message - too fragile

                // Ensure we tried to find out the version
                detector.Verify(x => x.DetectSchemaVersion(It.IsAny<XElement>()));
            }
        }

        [Test]
        public void FromXmlWrapsInternalExceptions()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            detector.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Throws<ArgumentNullException>();

            // Act
            try
            {
                converter.FromXml<Content>("<Test></Test>");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException ex)
            {
                // Don't bother checking actual error message - too fragile
                Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
            }
        }

        [Test]
        public void FromXmlConvertXmlBasedOnVersion()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();
            var engine = new Mock<IXmlMappingEngine>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            var expected = new Content { Name = "Fred" };

            detector.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("V1");
            factory.Setup(x => x.Find("V1")).Returns(engine.Object);
            engine.Setup(x => x.Map<XPathProcessor, Content>(It.IsAny<XPathProcessor>())).Returns(expected);

            // Act
            var candidate = converter.FromXml<Content>("<Test></Test>");

            // Assert
            Assert.AreSame(expected, candidate, "Not same");
        }

        [Test]
        public void FromXmlConvertXElementBasedOnVersion()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();
            var engine = new Mock<IXmlMappingEngine>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            var expected = new Content { Name = "Fred" };

            detector.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("V1");
            factory.Setup(x => x.Find("V1")).Returns(engine.Object);
            engine.Setup(x => x.Map<XPathProcessor, Content>(It.IsAny<XPathProcessor>())).Returns(expected);

            // Act
            var candidate = converter.FromXml<Content>(new XElement("Test"));

            // Assert
            Assert.AreSame(expected, candidate, "Not same");
        }

        [Test]
        public void FromXmlFailOnUnregisteredEngineVersion()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            detector.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("V1");

            // Act
            try
            {
                converter.FromXml<Content>("<Test></Test>");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException)
            {
                // Assert
                // Don't bother checking actual error message - too fragile

                // Ensure we asked for the correct version.
                factory.Verify(x => x.Find("V1"));
            }
        }

        [Test]
        public void ToXmlGeneratesXml()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();
            var engine = new Mock<IXmlMappingEngine>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            var element = new XElement("Test");
            var expected = element.ToXmlString();

            factory.Setup(x => x.Find("V1")).Returns(engine.Object);
            engine.Setup(x => x.CreateDocument(It.IsAny<Content>())).Returns(element);

            // Act
            var candidate = converter.ToXml(new Content(), "V1");

            // Assert
            Assert.AreEqual(expected, candidate, "XML differs");
        }

        [Test]
        public void ToXmlNullOnNullEntity()
        {
            // Arrange
            // NOTE Strict mocks to ensure we're not called.
            var detector = new Mock<IXmlVersionDetector>(MockBehavior.Strict);
            var factory = new Mock<IXmlMappingEngineFactory>(MockBehavior.Strict);

            var converter = new XmlConverter(detector.Object, factory.Object);

            // Act
            var candidate = converter.ToXml<Content>(null, "V1");

            // Assert
            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXmlFailOnUnregisteredEngineVersion()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();
            var engine = new Mock<IXmlMappingEngine>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            factory.Setup(x => x.Find("V1")).Returns(engine.Object);
            engine.Setup(x => x.CreateDocument(It.IsAny<Content>())).Throws<ArgumentNullException>();

            // Act
            try
            {
                converter.ToXml(new Content(), "V1");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException)
            {
                // Assert
                // Don't bother checking actual error message - too fragile

                // Ensure we asked for the correct version.
                factory.Verify(x => x.Find("V1"));
            }
        }

        [Test]
        public void ToXmlWrapsEngineFactoryException()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            factory.Setup(x => x.Find("V1")).Throws<ArgumentNullException>();

            // Act
            try
            {
                converter.ToXml(new Content(), "V1");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException ex)
            {
                Assert.AreEqual("IXmlMappingEngine not found: V1", ex.Message);
                Assert.IsInstanceOf<ArgumentNullException>(ex.InnerException);
            }
        }

        [Test]
        public void ToXmlPreservesEngineFactoryMappingException()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            factory.Setup(x => x.Find("V1")).Throws(new MappingException("Hello world"));

            // Act
            try
            {
                converter.ToXml(new Content(), "V1");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException ex)
            {
                Assert.AreEqual("Hello world", ex.Message);
            }
        }

        [Test]
        public void ToXmlWrapsEngineException()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();
            var engine = new Mock<IXmlMappingEngine>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            factory.Setup(x => x.Find("V1")).Returns(engine.Object);
            engine.Setup(x => x.CreateDocument(It.IsAny<Content>())).Throws(new ArgumentNullException());

            // Act
            try
            {
                converter.ToXml(new Content(), "V1");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException ex)
            {
                Assert.AreEqual("Could not convert entity to XML, version V1: Content", ex.Message);
                Assert.IsInstanceOf(typeof(ArgumentNullException), ex.InnerException);
            }
        }

        [Test]
        public void ToXmlPreservesEngineMappingException()
        {
            // Arrange
            var detector = new Mock<IXmlVersionDetector>();
            var factory = new Mock<IXmlMappingEngineFactory>();
            var engine = new Mock<IXmlMappingEngine>();

            var converter = new XmlConverter(detector.Object, factory.Object);

            factory.Setup(x => x.Find("V1")).Returns(engine.Object);
            engine.Setup(x => x.CreateDocument(It.IsAny<Content>())).Throws(new MappingException("Hello world"));

            // Act
            try
            {
                converter.ToXml(new Content(), "V1");

                throw new Exception("Should be MappingException");
            }
            catch (MappingException ex)
            {
                Assert.AreEqual("Hello world", ex.Message);
            }
        }

        public class Content
        {
            public string Name { get; set; }
        }
    }
}