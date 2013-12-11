namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class LinqXPathProcessorFixture : XPathProcessorFixture
    {
        private LinqXPathProcessor xp;

        [TestMethod]
        public void PushSingleNameSpaceReportsCurrentElementCorrectly()
        {
            var processor = XPathProcessor(Xml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");

            Assert.AreEqual(@"Fred", xp.CurrentElement.Name.LocalName);
        }

        [TestMethod]
        public void PushPopGetsCorrectCurrentElement()
        {
            var processor = XPathProcessor(Xml);
            processor.RegisterNamespace("sample", "http://sample.com");
            processor.RegisterNamespace("test", "http://test.com");

            processor.Push("Fred", "http://sample.com");
            processor.Push("Jim", "http://test.com");
            Assert.AreEqual(@"Jim", xp.CurrentElement.Name.LocalName);
            processor.Pop();
            Assert.AreEqual(@"Fred", xp.CurrentElement.Name.LocalName);
        }

        protected override XPathProcessor XPathProcessor(string xml)
        {
            var processor = new LinqXPathProcessor();
            processor.Initialize(xml);

            xp = processor;

            return processor;
        }
    }
}