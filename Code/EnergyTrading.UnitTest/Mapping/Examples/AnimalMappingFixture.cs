namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public abstract class AnimalMappingFixture<T> : MappingFixture<T>
        where T : class, new()
    {
        protected override void RegisterChildMappers(IXmlMappingEngine engine)
        {
            this.RegisterContainerMapper<Owner, OwnerXmlMapper>();
            this.RegisterContainerMapper<Animal, AnimalXmlMapper>();
            this.RegisterContainerMapper<Dog, DogXmlMapper>();
            this.RegisterContainerMapper<Cat, CatXmlMapper>();
        }
    }
}
