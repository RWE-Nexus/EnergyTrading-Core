namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class IdentifierXmlMapperFixture : MappingFixture<Identifier>
    {
        protected override string CreateExpectedXml()
        {
            return @"<identifier xmlns='http://www.sample.com/app' scheme='a'>b</identifier>";
        }

        protected override Identifier CreateExpectedDto()
        {
            return new Identifier
            {
                Scheme = "a",
                Value = "b"
            };
        }

        protected override XmlMapper<Identifier> CreateMapper(IXmlMappingEngine engine)
        {
            return new IdentifierXmlMapper();
        }
    }
}