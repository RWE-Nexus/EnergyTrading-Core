namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class LinqXmlMappingEngineFixture : XmlMappingEngineFixture
    {
        protected override XPathProcessor CreateProcessor()
        {
            var processor = new LinqXPathProcessor();

            return processor;
        }
    }
}