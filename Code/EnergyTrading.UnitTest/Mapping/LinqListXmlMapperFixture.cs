namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class LinqListXmlMapperFixture : ListXmlMapperFixture
    {
        protected override XPathProcessor CreateXPathProcessor()
        {
            return new LinqXPathProcessor();
        }
    }
}