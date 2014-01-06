namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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