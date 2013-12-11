namespace EnergyTrading.UnitTest.Mapping
{
    using System;
    using System.Xml.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Mapping;

    [TestClass]
    public class XmlVersionDetectorFixture : Fixture
    {
        private string sampleXml = "<hello></hello>";

        [TestMethod]
        public void ChildDetectorDetectsSchema()
        {
            // Arrange
            var mockValidator = new Mock<IXmlVersionDetector>();
            var validator = new XmlVersionDetector(new[] { mockValidator.Object });
            var xml = "<test></test>";

            mockValidator.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("test");

            // Act
            var candidate = validator.DetectSchemaVersion(xml);

            // Assert
            Assert.AreEqual("test", candidate);
        }

        [TestMethod]
        public void InvalidXmlRaisesException()
        {
            // Arrange
            var mockValidator = new Mock<IXmlVersionDetector>();
            var validator = new XmlVersionDetector(new[] { mockValidator.Object });
            var xml = "<test></tests>";

            // Act
            try
            {
                validator.DetectSchemaVersion(xml);

                throw new NotImplementedException("No exception");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void DetectorReturnsChildEmpty()
        {
            // Arrange
            var mockValidator = new Mock<IXmlVersionDetector>();
            var validator = new XmlVersionDetector(new[] { mockValidator.Object });
            var xml = "<test></test>";

            // Act
            var candidate = validator.DetectSchemaVersion(xml);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void DetectorTranslatesDetectorSchemaUnknown()
        {
            // Arrange
            var mockValidator = new Mock<IXmlVersionDetector>();
            var validator = new XmlVersionDetector(new[] { mockValidator.Object });
            var xml = "<test></test>";

            mockValidator.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("test.Unknown");

            // Act
            var candidate = validator.DetectSchemaVersion(xml);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void FirstRespondingDetectorDetectsSchema()
        {
            // Arrange
            var mockValidator = new Mock<IXmlVersionDetector>();
            var mockValidator2 = new Mock<IXmlVersionDetector>();
            var validator = new XmlVersionDetector(new[] { mockValidator.Object, mockValidator2.Object });
            var xml = "<test></test>";

            mockValidator2.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("test");

            // Act
            var candidate = validator.DetectSchemaVersion(xml);

            // Assert
            Assert.AreEqual("test", candidate);
        }

        [TestMethod]
        public void IgnoreDetectorException()
        {
            // Arrange
            var mockValidator = new Mock<IXmlVersionDetector>();
            var mockValidator2 = new Mock<IXmlVersionDetector>();
            var validator = new XmlVersionDetector(new[] { mockValidator.Object, mockValidator2.Object });
            var xml = "<test></test>";

            mockValidator.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Throws<NotImplementedException>();
            mockValidator2.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns("test");

            // Act
            var candidate = validator.DetectSchemaVersion(xml);

            // Assert
            Assert.AreEqual("test", candidate);
        }

        [TestMethod]
        public void DetectorResponseToNull()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>(MockBehavior.Strict);
            var detector = new XmlVersionDetector(new[] { d1.Object });

            string result = null;
            d1.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(result);
            
            // Act
            var candidate = detector.DetectSchemaVersion(sampleXml);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void DetectorResponseToEmptyString()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>();
            var detector = new XmlVersionDetector(new[] { d1.Object });

            // Act
            var candidate = detector.DetectSchemaVersion(string.Empty);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DetectorResponseToInvalidXml()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>();
            var detector = new XmlVersionDetector(new[] { d1.Object });

            // Act
            var candidate = detector.DetectSchemaVersion("<test>");

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void DetectorResponseToStringEmpty()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>();
            var detector = new XmlVersionDetector(new[] { d1.Object });

            string result = string.Empty;
            d1.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(result);

            // Act
            var candidate = detector.DetectSchemaVersion(sampleXml);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void DetectorResponseToUnknown()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>();
            var detector = new XmlVersionDetector(new[] { d1.Object });

            string result = "Unknown";
            d1.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(result);

            // Act
            var candidate = detector.DetectSchemaVersion(sampleXml);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void DetectorResponseToSchemaUnknown()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>();
            var detector = new XmlVersionDetector(new[] { d1.Object });

            string result = "Cts.Unknown";
            d1.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(result);

            // Act
            var candidate = detector.DetectSchemaVersion(sampleXml);

            // Assert
            Assert.AreEqual(string.Empty, candidate);
        }

        [TestMethod]
        public void DetectorResponseToVersion()
        {
            // Arrange
            var d1 = new Mock<IXmlVersionDetector>();
            var detector = new XmlVersionDetector(new[] { d1.Object });

            string result = "Cts.V2_1";
            d1.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(result);

            // Act
            var candidate = detector.DetectSchemaVersion(sampleXml);

            // Assert
            Assert.AreEqual(result, candidate);
        }
    }
}