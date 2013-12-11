namespace EnergyTrading.UnitTest.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;
    using EnergyTrading.Test.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;
    using EnergyTrading.Xml.Serialization;

    [TestClass]
    public class XmlMappingEngineFixture : XmlFixture
    {
        [TestMethod]
        public void CreateDocument()
        {
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Parent>, ParentXPathMapper>();
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Child>, ChildXPathMapper>();
            this.Container.RegisterType<IXmlMapper<Parent, XElement>, ParentXElementMapper>();
            this.Container.RegisterType<IXmlMapper<Child, XElement>, ChildXElementMapper>();

            this.Container.RegisterXmlMapper<Animal, AnimalXmlMapper>();
            this.Container.RegisterXmlMapper<Dog, DogXmlMapper>();

            new XmlMappingEngineRegistrar().Register(this.Container);

            var engine = ServiceLocator.GetInstance<IXmlMappingEngine>();

            var entity = new Dog { Id = 1, Name = "Test", Tricks = "Rollover" };

            var xml = engine.CreateDocument(entity);
        }

        [TestMethod]
        public void RoundTripWithOneWayMappers()
        {
            var locator = this.Container.Resolve<IServiceLocator>();

            this.Container.RegisterType<IXmlMapper<XPathProcessor, Parent>, ParentXPathMapper>();
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Child>, ChildXPathMapper>();
            this.Container.RegisterType<IXmlMapper<Parent, XElement>, ParentXElementMapper>();
            this.Container.RegisterType<IXmlMapper<Child, XElement>, ChildXElementMapper>();

            this.Container.RegisterXmlMapper<Animal, AnimalXmlMapper>();
            this.Container.RegisterXmlMapper<Dog, DogXmlMapper>();

            new XmlMappingEngineRegistrar().Register(this.Container);

            var engine = locator.GetInstance<IXmlMappingEngine>();
            this.RoundTripMapping(engine);
        }

        [TestMethod]
        public void RoundTripWithTwoWayMappers()
        {
            var locator = this.Container.Resolve<IServiceLocator>();

            this.Container.RegisterXmlMapper<Parent, ParentXmlMapper>();
            this.Container.RegisterXmlMapper<Child, ChildXmlMapper>();
            this.Container.RegisterXmlMapper<Animal, AnimalXmlMapper>();
            this.Container.RegisterXmlMapper<Dog, DogXmlMapper>();

            new XmlMappingEngineRegistrar().Register(this.Container);

            var engine = locator.GetInstance<IXmlMappingEngine>();
            this.RoundTripMapping(engine);
        }

        protected virtual XPathProcessor CreateProcessor()
        {
            var processor = new XPathProcessor();

            return processor;
        }

        protected void RoundTripMapping(IXmlMappingEngine engine)
        {
            // Sample xml with namespaces to add complexity
            var xml = @"<Parent xmlns='http://www.sample.com/common' xmlns:Pet='http://www.sample.com/pet' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                           <Id>1</Id>
                           <Name>Fred</Name>
                           <Cost>1500.75</Cost>
                           <Children xmlns='http://www.sample.com/app'>
                               <Child>
                                   <Id>1</Id>
                                   <Value xmlns='http://www.sample.com/sales'>34.29</Value>
                                   <Start>2011-09-15T15:00:00</Start>
                                    <Animal xmlns='http://www.sample.com/app' xsi:type='Pet:Dog'>
                                        <Id>1</Id>
                                        <Name>Fido</Name>
                                        <Pet:Tricks>Fetch</Pet:Tricks>
                                    </Animal>
                               </Child>
                               <Child>
                                   <Id>2</Id>
                                   <Value xmlns='http://www.sample.com/sales'>12</Value>
                                   <Start>2020-12-31T04:00:00</Start>
                               </Child>
                           </Children>
                        </Parent>";

            var d = new Dog { Id = 1, Name = "Fido", Tricks = "Fetch" };
            var expected = new Parent
            {
                Id = 1,
                Name = "Fred",
                Cost = 1500.75M,
                Children = new List<Child>
                {
                     new Child { Id = 1, Value = 34.29f, Start = new DateTime(2011, 9, 15, 15, 0, 0), Dog = d },  
                     new Child { Id = 2, Value = 12f, Start = new DateTime(2020, 12, 31, 4, 0, 0) },
                }
            };

            var processor = this.CreateProcessor();
            processor.Initialize(xml);

            var candidate = engine.Map<XPathProcessor, Parent>(processor);
            this.Check(expected, candidate);

            var candidateXml = engine.Map<Parent, XElement>(candidate);
            this.CheckXml(candidateXml, xml);
        }

        protected new void CheckXml(XElement node, string xml, bool fullComparison = true)
        {
            var source = new XmlDocument();
            source.LoadXml(xml);
            var expected = source.SelectSingleNode("/*");

            var target = new XDocument(node).ToXmlDocument();
            var candidate = target.SelectSingleNode("/*");

            using (var sw = new StringWriter())
            {
                bool noDifference;
                using (var writer = new XmlTextWriter(sw) { Formatting = Formatting.Indented })
                {
                    var diff = XmlDiffFactory.DefaultDiffEngine();

                    noDifference = diff.Compare(expected, candidate, writer);
                }

                if (!noDifference)
                {
                    Assert.Fail("Xml differs:\r\n " + sw);
                }
            }
        }
    }
}