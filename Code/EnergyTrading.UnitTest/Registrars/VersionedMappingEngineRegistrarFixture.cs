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
    public class VersionedMappingEngineRegistrarFixture
    {
        [TestMethod]
        public void ResolveV1EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new AnimalMappingEngineRegistrar().Register(container);

            var engine = container.Resolve<IMappingEngine>("V1");

            engine.ResolveMapper<Owner, Entity, Maps.OwnerEntityMapper>();
            engine.ResolveMapper<Entity, Entity, Maps.Common.EntityEntityMapper>();
            engine.ResolveMapper<Animal, AnimalModel, Maps.Common.V1.AnimalAnimalModelMapper>();
            engine.ResolveMapper<Animal, AnimalModel, Maps.Common.V1.AnimalAnimalModelMapper>();
            engine.ResolveMapper<Child, ChildModel, Maps.Common.V1.ChildChildModelMapper>();
        }

        [TestMethod]
        public void ResolveV2EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new XmlSchemaRegistryRegistrar().Register(container);
            new XmlMappingEngineRegistrar().Register(container);
            new XmlMappingEngineFactoryRegistrar().Register(container);

            new AnimalMappingEngineRegistrar().Register(container);

            var engine = container.Resolve<IMappingEngine>("V2");

            engine.ResolveMapper<Owner, Entity, Maps.OwnerEntityMapper>();
            engine.ResolveMapper<Entity, Entity, Maps.Common.EntityEntityMapper>();
            engine.ResolveMapper<Animal, AnimalModel, Maps.Common.V2.AnimalAnimalModelMapper>();
            engine.ResolveMapper<Child, ChildModel, Maps.Common.V1.ChildChildModelMapper>();
        }

        [TestMethod]
        public void ResolveV2R1EngineAndMappers()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            new AnimalMappingEngineRegistrar().Register(container);

            var engine = container.Resolve<IMappingEngine>("V2_1");

            engine.ResolveMapper<Owner, Entity, Maps.OwnerEntityMapper>();
            engine.ResolveMapper<Entity, Entity, Maps.Common.EntityEntityMapper>();
            engine.ResolveMapper<Animal, AnimalModel, Maps.Common.V2_1.AnimalAnimalModelMapper>();
            engine.ResolveMapper<Child, ChildModel, Maps.Common.V1.ChildChildModelMapper>();
        }
    }
}