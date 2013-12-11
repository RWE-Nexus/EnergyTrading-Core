namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class LinqXmlMapperFixture : XmlMapperFixture
    {
        protected override XPathProcessor CreateProcessor()
        {
            var processor = new LinqXPathProcessor();

            return processor;
        }
    }
}