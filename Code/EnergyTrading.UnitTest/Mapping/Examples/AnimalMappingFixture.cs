namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public abstract class AnimalMappingFixture<T> : MappingFixture<T>
        where T : class, new()
    {
        protected override void RegisterChildMappers(IXmlMappingEngine engine)
        {
            RegisterContainerMapper<Owner, OwnerXmlMapper>();
            RegisterContainerMapper<Animal, AnimalXmlMapper>();
            RegisterContainerMapper<Dog, DogXmlMapper>();
            RegisterContainerMapper<Cat, CatXmlMapper>();
        }
    }
}
