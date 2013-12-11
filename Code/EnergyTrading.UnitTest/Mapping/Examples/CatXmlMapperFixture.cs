namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public abstract class CatXmlMapperFixture : AnimalMappingFixture<Cat>
    {
        protected override XmlMapper<Cat> CreateMapper(IXmlMappingEngine engine)
        {
            return new CatXmlMapper(engine);
        }
    }
}