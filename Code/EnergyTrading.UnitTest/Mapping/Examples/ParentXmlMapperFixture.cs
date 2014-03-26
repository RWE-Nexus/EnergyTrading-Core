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
            engine.RegisterMapper(new OwnerXmlMapper(engine));
            engine.RegisterMapper(new AnimalXmlMapper(engine));
            engine.RegisterMapper(new DogXmlMapper(engine));
            engine.RegisterMapper(new CatXmlMapper(engine));
        }
    }
}