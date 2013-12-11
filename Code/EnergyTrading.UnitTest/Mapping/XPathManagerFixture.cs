namespace EnergyTrading.UnitTest.Mapping
{
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class XPathManagerFixture
    {
        protected INamespaceManager NamespaceManager { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            NamespaceManager = this.CreateNamespaceManager();
            NamespaceManager.RegisterNamespace("a", "http://www.a.com");
            NamespaceManager.RegisterNamespace("b", "http://www.b.com");
        }
        
        [TestMethod]
        public void EmptyPrefixNamespace()
        {
            var manager = this.CreateXPathManager();

            var expected = "fred";
            var candidate = manager.QualifyXPath("fred", null);

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void PrefixQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred";
            var candidate = manager.QualifyXPath("fred", "a");

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void PrefixPreferenceQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred";
            var candidate = manager.QualifyXPath("fred", "a", "http://www.b.com");

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void NamespaceQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred";
            var candidate = manager.QualifyXPath("fred", null, "http://www.a.com");

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void AttributeXPath()
        {
            var manager = this.CreateXPathManager();

            var expected = "@a:fred";
            var candidate = manager.QualifyXPath("fred", "a", null, -1, true);

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void IndexedXPath()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred[0]";
            var candidate = manager.QualifyXPath("fred", "a", null, 0);

            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        [ExpectedException(typeof(MappingException))]
        public void UnregisteredNamespace()
        {
            var manager = this.CreateXPathManager();

            manager.QualifyXPath("fred", null, "http://www.c.com");
        }

        protected virtual IXPathManager CreateXPathManager()
        {            
            var xp = new XPathManager(NamespaceManager);

            return xp;
        }

        protected virtual INamespaceManager CreateNamespaceManager()
        {
            var ns = new XmlNamespaceManager(new NameTable());

            return new BaseNamespaceManager(ns);
        }
    }
}