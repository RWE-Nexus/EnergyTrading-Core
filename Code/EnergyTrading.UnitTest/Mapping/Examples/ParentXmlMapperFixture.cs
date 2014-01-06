namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;
    using EnergyTrading.Test.Mapping;

    public abstract class ParentXmlMapperFixture : MappingFixture<Parent>
    {
        protected override XmlMapper<Parent> CreateMapper(IXmlMappingEngine engine)
        {
            return new ParentXmlMapper(engine);
        }

        protected override void RegisterChildMappers(IXmlMappingEngine engine)
        {
            this.RegisterContainerMapper<Child, ChildXmlMapper>();
            this.RegisterContainerMapper<Animal, AnimalXmlMapper>();
            this.RegisterContainerMapper<Dog, DogXmlMapper>();
            this.RegisterContainerMapper<Cat, CatXmlMapper>();
        }
    }
}