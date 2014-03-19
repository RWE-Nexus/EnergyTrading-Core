namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public abstract class AnimalMappingFixture<T> : MappingFixture<T>
        where T : class, new()
    {
        protected override void RegisterChildMappers(IXmlMappingEngine engine)
        {
            engine.RegisterMapper(new OwnerXmlMapper(engine));
            engine.RegisterMapper(new AnimalXmlMapper(engine));
            engine.RegisterMapper(new DogXmlMapper(engine));
            engine.RegisterMapper(new CatXmlMapper(engine));
        }
    }
}