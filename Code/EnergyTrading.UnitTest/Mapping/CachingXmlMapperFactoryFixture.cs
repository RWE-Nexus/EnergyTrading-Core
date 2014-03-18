namespace EnergyTrading.UnitTest.Mapping
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class CachingXmlMapperFactoryFixture : Fixture
    {
        [Test]
        public void FindMapperViaGeneric()
        {
            var locator = new Mock<IServiceLocator>();
            var innerFactory = new Mock<IXmlMapperFactory>();
            var factory = new CachingXmlMapperFactory(innerFactory.Object);
            var mapperFactory = new LocatorXmlMapperFactory(locator.Object);
            var engine = new XmlMappingEngine(mapperFactory);

            var type = typeof(IXmlMapper<Parent, XElement>);
            innerFactory.Setup(x => x.Mapper(typeof(Parent), typeof(XElement), null)).Returns(new ParentXmlMapper(engine));
            locator.Setup(x => x.GetInstance(type)).Returns(new ParentXmlMapper(engine));

            var candidate = factory.Mapper<Parent, XElement>();
            Assert.IsNotNull(candidate);
            Assert.IsTrue(candidate is ParentXmlMapper);

            // Call again
            factory.Mapper<Parent, XElement>();
            innerFactory.Verify(x => x.Mapper(typeof(Parent), typeof(XElement), null), Times.Once());
        }

        [Test]
        public void FindMapperViaTypes()
        {
            var locator = new Mock<IServiceLocator>();
            var innerFactory = new Mock<IXmlMapperFactory>();
            var factory = new CachingXmlMapperFactory(innerFactory.Object);
            var mapperFactory = new LocatorXmlMapperFactory(locator.Object);
            var engine = new XmlMappingEngine(mapperFactory);

            var type = typeof(IXmlMapper<Parent, XElement>);
            innerFactory.Setup(x => x.Mapper(typeof(Parent), typeof(XElement), null)).Returns(new ParentXmlMapper(engine));
            locator.Setup(x => x.GetInstance(type)).Returns(new ParentXmlMapper(engine));

            var candidate = factory.Mapper(typeof(Parent), typeof(XElement));
            Assert.IsNotNull(candidate);
            Assert.IsTrue(candidate is ParentXmlMapper);

            factory.Mapper(typeof(Parent), typeof(XElement));
            innerFactory.Verify(x => x.Mapper(typeof(Parent), typeof(XElement), null), Times.Once());
        }
    }
}