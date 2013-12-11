namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class XDocumentProcessorFixture : XPathProcessorFixture
    {
        protected override XPathProcessor XPathProcessor(string xml)
        {
            var processor = new XDocumentXPathProcessor();
            processor.Initialize(xml);
            return processor;
        }
    }
}