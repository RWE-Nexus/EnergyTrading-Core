namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Mapping;
    using EnergyTrading.Test.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Registrars;

    using AnimalXmlMapper = EnergyTrading.UnitTest.Registrars.Maps.Common.V1.AnimalXmlMapper;
    using ChildXmlMapper = EnergyTrading.UnitTest.Registrars.Maps.Common.V1.ChildXmlMapper;
    using EntityXmlMapper = EnergyTrading.UnitTest.Registrars.Maps.Common.EntityXmlMapper;
    using OwnerXmlMapper = EnergyTrading.UnitTest.Registrars.Maps.OwnerXmlMapper;

    [TestFixture]
    public class VersionedXmlMappingEngineRegistrarFixture
    {
        [Test]
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

            engine.ResolveMapper<XPathProcessor, Owner, OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, ChildXmlMapper>();
        }

        [Test]
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

            engine.ResolveMapper<XPathProcessor, Owner, OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V2.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, ChildXmlMapper>();
        }

        [Test]
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

            engine.ResolveMapper<XPathProcessor, Owner, OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V2_1.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, ChildXmlMapper>();
        }

        [Test]
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

            engine.ResolveMapper<XPathProcessor, Owner, OwnerXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Entity, EntityXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Animal, Maps.Common.V2_1.AnimalXmlMapper>();
            engine.ResolveMapper<XPathProcessor, Child, ChildXmlMapper>();
        }
    }
}