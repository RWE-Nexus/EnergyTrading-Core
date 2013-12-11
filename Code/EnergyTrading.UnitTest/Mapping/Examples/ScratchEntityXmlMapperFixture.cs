namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public abstract class ScratchEntityXmlMapperFixture : MappingFixture<ScratchEntity>
    {        
        protected override XmlMapper<ScratchEntity> CreateMapper(IXmlMappingEngine engine)
        {
            return new ScratchEntityXmlMapper();
        }
    }
}