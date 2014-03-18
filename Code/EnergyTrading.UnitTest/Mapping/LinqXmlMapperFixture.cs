namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class LinqXmlMapperFixture : XmlMapperFixture
    {
        protected override XPathProcessor CreateProcessor()
        {
            var processor = new LinqXPathProcessor();

            return processor;
        }
    }
}