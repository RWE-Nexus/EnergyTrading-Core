namespace EnergyTrading.UnitTest.Mapping
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Mapping;

    [TestClass]
    public class CompatibleXmlMappingEngineFactoryFixture
    {
        [TestMethod]
        public void TryFindReturnsEngineIfPresent()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object;
            f.Setup(x => x.TryFind("Css.V10_2", out e)).Returns(true);

            IXmlMappingEngine candidate;
            Assert.IsTrue(factory.TryFind("Css.V10_2", out candidate));
            Assert.AreEqual(engine.Object, candidate);
        }

        [TestMethod]
        public void TryFindReturnsNullIfNotPresent()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            IXmlMappingEngine candidate;
            Assert.IsFalse(factory.TryFind("Css.V10_2", out candidate));
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void FindThrowsUnexpectedSchema()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            try
            {
                factory.Find("Css.V10_2");

                throw new NotImplementedException("Should throw XmlEngineResolutionException");
            }
            catch (XmlEngineResolutionException ex)
            {
                Assert.AreEqual(XmlEngineResolutionErrorCode.UnexpectedSchema, ex.Code);
            }
        }

        [TestMethod]
        public void FindThrowsVersionTooHigh()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            s.Setup(x => x.SchemaExists("Css")).Returns(true);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object;
            f.Setup(x => x.TryFind("Css.V9", out e)).Returns(true);

            try
            {
                factory.Find("Css.V10_2");

                throw new NotImplementedException("Should throw XmlEngineResolutionException");
            }
            catch (XmlEngineResolutionException ex)
            {
                Assert.AreEqual(XmlEngineResolutionErrorCode.MessageVersionTooHigh, ex.Code);
            }
        }

        [TestMethod]
        public void FindThrowsVersionTooLow()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            s.Setup(x => x.SchemaExists("Css")).Returns(true);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object;
            f.Setup(x => x.TryFind("Css.V11", out e)).Returns(true);

            try
            {
                factory.Find("Css.V10_2");

                throw new NotImplementedException("Should throw XmlEngineResolutionException");
            }
            catch (XmlEngineResolutionException ex)
            {
                Assert.AreEqual(XmlEngineResolutionErrorCode.MessageVersionTooLow, ex.Code);
            }
        }

        [TestMethod]
        public void FindThrowsIfAsmStringInvalid()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            s.Setup(x => x.SchemaExists("Css")).Returns(true);

            try
            {
                factory.Find("Css.V10.1");

                throw new NotImplementedException("Should throw MappingException");
            }
            catch (XmlEngineResolutionException ex)
            {
                Assert.AreEqual(XmlEngineResolutionErrorCode.Undetermined, ex.Code);
            }
        }

        [TestMethod]
        public void FindReturnsExactEngineIfPresent()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object;
            f.Setup(x => x.TryFind("Css.V10_2", out e)).Returns(true);

            var candidate = factory.Find("Css.V10_2");
            Assert.AreEqual(engine.Object, candidate);
        }

        [TestMethod]
        public void FindHandlesNonSchemaVersioning()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object;
            f.Setup(x => x.TryFind("V10_1", out e)).Returns(true);

            var candidate = factory.Find("V10_2");
            Assert.AreEqual(engine.Object, candidate);
        }

        [TestMethod]
        public void FindReturnsLowerMinorVersion()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object; 
            f.Setup(x => x.TryFind("Css.V10", out e)).Returns(true);

            var candidate = factory.Find("Css.V10_2");
            Assert.AreEqual(engine.Object, candidate);
        }

        [TestMethod]
        public void FindReturnsHighestLowerMinorVersion()
        {
            var f = new Mock<IXmlMappingEngineFactory>();
            var s = new Mock<IXmlSchemaRegistry>();
            var factory = new CompatibleXmlMappingEngineFactory(f.Object, s.Object);

            var engine = new Mock<IXmlMappingEngine>();
            var e = engine.Object;
            var engine2 = new Mock<IXmlMappingEngine>();
            var e2 = engine2.Object;
            f.Setup(x => x.TryFind("Css.V10_1", out e)).Returns(true);
            f.Setup(x => x.TryFind("Css.V10", out e2)).Returns(true);

            var candidate = factory.Find("Css.V10_2");
            Assert.AreEqual(engine.Object, candidate);
        }
    }
}