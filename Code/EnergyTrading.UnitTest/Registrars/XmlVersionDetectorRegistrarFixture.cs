namespace EnergyTrading.UnitTest.Registrars
{
    using System;
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;

    [TestClass]
    public class XmlVersionDetectorRegistrarFixture
    {
        [TestMethod]
        public void TestThatItemsAreResolvedFromContainer()
        {
            Func<XElement, string> func1 = (x) => x.Value == "1" ? "1.0" : "Unknown";
            Func<XElement, string> func2 = (x) => x.Value == "2" ? "2.0" : "Unknown";
            var container = new UnityContainer();
            var mock1 = new Mock<IXmlVersionDetector>();
            var mock2 = new Mock<IXmlVersionDetector>();
            mock1.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(func1);
            mock2.Setup(x => x.DetectSchemaVersion(It.IsAny<XElement>())).Returns(func2);
            container.RegisterInstance("1", mock1.Object);
            container.RegisterInstance("2", mock2.Object);
            new XmlVersionDetectorRegistrar().Register(container);
            var versionDetector = container.Resolve<IXmlVersionDetector>();
            Assert.AreEqual("1.0", versionDetector.DetectSchemaVersion("<version>1</version>"));
            Assert.AreEqual("2.0", versionDetector.DetectSchemaVersion("<version>2</version>"));
            Assert.AreEqual(string.Empty, versionDetector.DetectSchemaVersion("<version>3</version>"));
        }
    }
}
