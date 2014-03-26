namespace EnergyTrading.UnitTest.Mapping
{
    using System.Xml;

    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class XPathManagerFixture
    {
        protected INamespaceManager NamespaceManager { get; set; }

        [SetUp]
        public void Initialize()
        {
            this.NamespaceManager = this.CreateNamespaceManager();
            this.NamespaceManager.RegisterNamespace("a", "http://www.a.com");
            this.NamespaceManager.RegisterNamespace("b", "http://www.b.com");
        }
        
        [Test]
        public void EmptyPrefixNamespace()
        {
            var manager = this.CreateXPathManager();

            var expected = "fred";
            var candidate = manager.QualifyXPath("fred", null);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void PrefixQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred";
            var candidate = manager.QualifyXPath("fred", "a");

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void PrefixPreferenceQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred";
            var candidate = manager.QualifyXPath("fred", "a", "http://www.b.com");

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void NamespaceQualified()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred";
            var candidate = manager.QualifyXPath("fred", null, "http://www.a.com");

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void AttributeXPath()
        {
            var manager = this.CreateXPathManager();

            var expected = "@a:fred";
            var candidate = manager.QualifyXPath("fred", "a", null, -1, true);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void IndexedXPath()
        {
            var manager = this.CreateXPathManager();

            var expected = "a:fred[0]";
            var candidate = manager.QualifyXPath("fred", "a", null, 0);

            Assert.AreEqual(expected, candidate);
        }

        [Test]
        [ExpectedException(typeof(MappingException))]
        public void UnregisteredNamespace()
        {
            var manager = this.CreateXPathManager();

            manager.QualifyXPath("fred", null, "http://www.c.com");
        }

        protected virtual IXPathManager CreateXPathManager()
        {            
            var xp = new XPathManager(this.NamespaceManager);

            return xp;
        }

        protected virtual INamespaceManager CreateNamespaceManager()
        {
            var ns = new XmlNamespaceManager(new NameTable());

            return new BaseNamespaceManager(ns);
        }
    }
}