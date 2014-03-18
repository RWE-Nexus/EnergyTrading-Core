namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
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