namespace EnergyTrading.UnitTest.Registrars
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;
    using EnergyTrading.Test.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    [TestClass]
    public class VersionedXmlMappingEngineRegistrarFixture
    {
        [TestMethod]
        public void ResolveV1EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new XmlSchemaRegistryRegistrar().Register(container);
            new XmlMappingEngineRegistrar().Register(container);
            new XmlMappingEngineFactoryRegistrar().Register(container);

            new AnimalXmlMappingEngineRegistrar().Register(container);

            var factory = container.Resolve<IXmlMappingEngineFactory>();
            var engine = factory.Find("Animal.V1");

            engine.ResolveMapper<XPathProcessor, Owner, Maps.OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, Maps.Common.EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V1.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, Maps.Common.V1.ChildXmlMapper>();
        }

        [TestMethod]
        public void ResolveV2EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new XmlSchemaRegistryRegistrar().Register(container);
            new XmlMappingEngineRegistrar().Register(container);
            new XmlMappingEngineFactoryRegistrar().Register(container);

            new AnimalXmlMappingEngineRegistrar().Register(container);

            var factory = container.Resolve<IXmlMappingEngineFactory>();
            var engine = factory.Find("Animal.V2");

            engine.ResolveMapper<XPathProcessor, Owner, Maps.OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, Maps.Common.EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V2.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, Maps.Common.V1.ChildXmlMapper>();
        }

        [TestMethod]
        public void ResolveV2R1EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new XmlSchemaRegistryRegistrar().Register(container);
            new XmlMappingEngineRegistrar().Register(container);
            new XmlMappingEngineFactoryRegistrar().Register(container);

            new AnimalXmlMappingEngineRegistrar().Register(container);

            var factory = container.Resolve<IXmlMappingEngineFactory>();
            var engine = factory.Find("Animal.V2_1");

            engine.ResolveMapper<XPathProcessor, Owner, Maps.OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, Maps.Common.EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V2_1.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, Maps.Common.V1.ChildXmlMapper>();
        }

        [TestMethod]
        public void TryResolveV2R2EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new XmlSchemaRegistryRegistrar().Register(container);
            new XmlMappingEngineRegistrar().Register(container);
            new XmlMappingEngineFactoryRegistrar().Register(container);

            new AnimalXmlMappingEngineRegistrar().Register(container);

            var factory = container.Resolve<IXmlMappingEngineFactory>();
            var engine = factory.Find("Animal.V2_2");

            engine.ResolveMapper<XPathProcessor, Owner, Maps.OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, Maps.Common.EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V2_1.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, Maps.Common.V1.ChildXmlMapper>();
        }
    }
}