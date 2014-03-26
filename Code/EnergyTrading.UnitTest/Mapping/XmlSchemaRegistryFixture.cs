namespace EnergyTrading.UnitTest.Mapping
{
    using System.Linq;

    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class XmlSchemaRegistryFixture
    {
        [Test]
        public void ExistsReportsMissingSchema()
        {
            // Arrange
            IXmlSchemaRegistry registry = new XmlSchemaRegistry();

            // Act
            var candidate = registry.SchemaExists("test");

            // Assert
            Assert.IsFalse(candidate, "Test schema present");
        }

        [Test]
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

        [Test]
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