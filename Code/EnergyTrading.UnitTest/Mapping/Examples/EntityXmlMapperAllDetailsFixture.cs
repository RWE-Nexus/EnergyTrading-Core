namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class EntityXmlMapperAllDetailsFixture : MappingFixture<Entity>
    {
        protected override string CreateExpectedXml()
        {
            return @"<Entity>
                        <Id>1</Id>
                        <Id2>A</Id2>
                        <Name>Test</Name>
                        <Name2>Bob</Name2>
                        <Total>3</Total>
                     </Entity>";
        }

        protected override Entity CreateExpectedDto()
        {
            return new Entity
            {
                Id = "1",
                Id2 = "A",
                Name = "Test",
                Name2 = "Bob",
                Total = 3
            };
        }

        protected override XmlMapper<Entity> CreateMapper(IXmlMappingEngine engine)
        {
            return new EntityXmlMapper();
        }
    }
}