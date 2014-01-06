namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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