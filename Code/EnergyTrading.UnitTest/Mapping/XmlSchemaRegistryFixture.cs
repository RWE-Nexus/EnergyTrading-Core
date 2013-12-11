namespace EnergyTrading.UnitTest.Mapping
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class XmlSchemaRegistryFixture
    {
        [TestMethod]
        public void ExistsReportsMissingSchema()
        {
            // Arrange
            IXmlSchemaRegistry registry = new XmlSchemaRegistry();

            // Act
            var candidate = registry.SchemaExists("test");

            // Assert
            Assert.IsFalse(candidate, "Test schema present");
        }

        [TestMethod]
        public void ExistsReportsSchema()
        {
            // Arrange
            IXmlSchemaRegistry registry = new XmlSchemaRegistry();
            registry.RegisterSchema("test");

            // Act
            var candidate = registry.SchemaExists("test");

            // Assert
            Assert.IsTrue(candidate, "Test schema not present");
        }

        [TestMethod]
        public void GetSchemasReportsAllSchemas()
        {
            // Arrange
            IXmlSchemaRegistry registry = new XmlSchemaRegistry();
            registry.RegisterSchema("test");

            // Act
            var candidate = registry.GetSchemas().ToList();

            // Assert
            Assert.IsNotNull(candidate);
            Assert.AreEqual(1, candidate.Count, "Count differs");
            Assert.AreEqual("test", candidate[0], "First element differs");
        }
    }
}