namespace EnergyTrading.UnitTest.Mapping
{
    using System;
    using System.Xml.Linq;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    [TestClass]
    public class LocatorXmlMapperFactoryFixture : Fixture
    {
        [TestMethod]
        public void FindMapperViaGeneric()
        {
            var locator = new Mock<IServiceLocator>();
            var factory = new LocatorXmlMapperFactory(locator.Object);
            var engine = new XmlMappingEngine(factory);

            var type = typeof(IXmlMapper<Parent, XElement>);
            locator.Setup(x => x.GetInstance(type, null)).Returns(new ParentXmlMapper(engine));

            var candidate = factory.Mapper<Parent, XElement>();
            Assert.IsNotNull(candidate);
            Assert.IsTrue(candidate is ParentXmlMapper);
        }

        [TestMethod]
        public void FindMapperViaTypes()
        {
            var locator = new Mock<IServiceLocator>();
            var factory = new LocatorXmlMapperFactory(locator.Object);
            var engine = new XmlMappingEngine(factory);

            var type = typeof(IXmlMapper<Parent, XElement>);
            locator.Setup(x => x.GetInstance(type, null)).Returns(new ParentXmlMapper(engine));

            var candidate = factory.Mapper(typeof(Parent), typeof(XElement));
            Assert.IsNotNull(candidate);
            Assert.IsTrue(candidate is ParentXmlMapper);
        }

        [TestMethod]
        public void LocatorFailureThrowsMappingException()
        {
            var locator = new Mock<IServiceLocator>();
            var factory = new LocatorXmlMapperFactory(locator.Object);

            var type = typeof(IXmlMapper<Parent, XElement>);
            // Checks we trap and raise the right sort of exception
            locator.Setup(x => x.GetInstance(type)).Throws(new ArgumentException("Failed"));

            try
            {
                factory.Mapper<Parent, XElement>();

                throw new NotSupportedException("Should raise MappingException");
            }
            catch (MappingException)
            {                
            }
        }

        [TestMethod]
        public void RegistrationNotSupported()
        {
            var locator = new Mock<IServiceLocator>();
            var factory = new LocatorXmlMapperFactory(locator.Object);

            var mapper = new ParentXmlMapper(null);

            try
            {
                factory.Register<Parent, XElement>(mapper);

                throw new NotImplementedException("Should raise NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }
        }
    }
}