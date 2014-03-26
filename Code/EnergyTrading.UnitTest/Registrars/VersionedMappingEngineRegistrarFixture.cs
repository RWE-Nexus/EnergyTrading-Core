namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Mapping;
    using EnergyTrading.Test.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;
    using EnergyTrading.UnitTest.Registrars.Maps;
    using EnergyTrading.UnitTest.Registrars.Maps.Common;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Registrars;

    using AnimalAnimalModelMapper = EnergyTrading.UnitTest.Registrars.Maps.Common.V1.AnimalAnimalModelMapper;
    using ChildChildModelMapper = EnergyTrading.UnitTest.Registrars.Maps.Common.V1.ChildChildModelMapper;

    [TestFixture]
    public class VersionedMappingEngineRegistrarFixture
    {
        [Test]
        public void ResolveV1EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new AnimalMappingEngineRegistrar().Register(container);

            var engine = container.Resolve<IMappingEngine>("V1");

            engine.ResolveMapper<Owner, Entity, OwnerEntityMapper>();
            engine.ResolveMapper<Entity, Entity, EntityEntityMapper>();
            engine.ResolveMapper<Animal, AnimalModel, AnimalAnimalModelMapper>();
            engine.ResolveMapper<Animal, AnimalModel, AnimalAnimalModelMapper>();
            engine.ResolveMapper<Child, ChildModel, ChildChildModelMapper>();
        }

        [Test]
        public void ResolveV2EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new XmlSchemaRegistryRegistrar().Register(container);
            new XmlMappingEngineRegistrar().Register(container);
            new XmlMappingEngineFactoryRegistrar().Register(container);

            new AnimalMappingEngineRegistrar().Register(container);

            var engine = container.Resolve<IMappingEngine>("V2");

            engine.ResolveMapper<Owner, Entity, OwnerEntityMapper>();
            engine.ResolveMapper<Entity, Entity, EntityEntityMapper>();
            engine.ResolveMapper<Animal, AnimalModel, Maps.Common.V2.AnimalAnimalModelMapper>();
            engine.ResolveMapper<Child, ChildModel, ChildChildModelMapper>();
        }

        [Test]
        public void ResolveV2R1EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new AnimalMappingEngineRegistrar().Register(container);

            var engine = container.Resolve<IMappingEngine>("V2_1");

            engine.ResolveMapper<Owner, Entity, OwnerEntityMapper>();
            engine.ResolveMapper<Entity, Entity, EntityEntityMapper>();
            engine.ResolveMapper<Animal, AnimalModel, Maps.Common.V2_1.AnimalAnimalModelMapper>();
            engine.ResolveMapper<Child, ChildModel, ChildChildModelMapper>();
        }
    }
}