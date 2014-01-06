namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LinqListXmlMapperFixture : ListXmlMapperFixture
    {
        protected override XPathProcessor CreateXPathProcessor()
        {
            return new LinqXPathProcessor();
        }
    }
}