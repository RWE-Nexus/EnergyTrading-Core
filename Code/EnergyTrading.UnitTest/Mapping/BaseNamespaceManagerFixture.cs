namespace EnergyTrading.UnitTest.Mapping
{
    using System;
    using System.Diagnostics;
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class BaseNamespaceManagerFixture
    {
        [TestMethod]
        public void LookupPrefix()
        {
            var uri = "http://www.test.org";
            var manager = this.CreateManager();
            manager.RegisterNamespace("a", uri);

            var candidate = manager.LookupPrefix(uri);

            Assert.AreEqual("a", candidate, "Prefix differs");
        }

        //[TestMethod]
        //public void RegisterInvalidPrefix()
        //{
        //    var uri = "http://www.test.org";
        //    var manager = this.CreateManager();

        //    try
        //    {
        //        manager.RegisterNamespace("a:", uri);

        //        throw new NotImplementedException("Should reject invalid character in prefix.");
        //    }
        //    catch (XmlException ex)
        //    {
        //        Assert.IsTrue(ex.Message.Contains("The ':' character, hexadecimal value 0x3A, cannot be included in a name"));
        //    }
        //}

        [TestMethod]
        public void LookupNamespace()
        {
            var uri = "http://www.test.org";
            var manager = this.CreateManager();
            manager.RegisterNamespace("a", uri);

            var candidate = manager.LookupNamespace("a");

            Assert.AreEqual(uri, candidate, "Namespace differs");
        }

        [TestMethod]
        public void LookupNamespaceQualified()
        {
            var uri = "http://www.test.org";
            var manager = this.CreateManager();
            manager.RegisterNamespace("a", uri);

            var candidate = manager.LookupNamespace("a", true);

            Assert.AreEqual("{" + uri + "}", candidate, "Namespace differs");
        }

        [TestMethod]
        public void IsPrefixDefined()
        {
            var uri = "http://www.test.org";
            var manager = this.CreateManager();
            manager.RegisterNamespace("a", uri);

            Assert.IsTrue(manager.PrefixExists("a"), "Prefix a doesn't exist");
            Assert.IsFalse(manager.PrefixExists("b"), "Prefix b exists");
        }

        [TestMethod]
        public void IsNamespaceDefined()
        {
            var uri = "http://www.test.org";
            var manager = this.CreateManager();
            manager.RegisterNamespace("a", uri);

            Assert.IsTrue(manager.NamespaceExists(uri), "Namespace test doesn't exist");
            Assert.IsFalse(manager.PrefixExists(uri + "/a"), "Namespace test/a exists");
        }

        [TestMethod]
        public void Performance()
        {
            var manager = this.CreateManager();
            var uris = new[] { "http://www.a.com", "http://www.b.com", "http://www.c.com", "http://www.d.com", "http://www.e.com", "http://www.f.com", "http://www.g.com", "http://www.h.com" };
            var prefixes = new[] { "a", "b", "c", "d", "e", "f", "g", "h" };
            var count = uris.GetUpperBound(0);

            for (var i = 0; i < count; i++)
            {
                manager.RegisterNamespace(prefixes[i], uris[i]);
            }

            var rnd = new Random();
            var sw = new Stopwatch();

            sw.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var j = rnd.Next(count);

                var pf = manager.LookupPrefix(uris[j]);
                var ns = manager.LookupNamespace(prefixes[j]);
                var nsq = manager.LookupNamespace(prefixes[j], true);
            }

            sw.Stop();
            var result = sw.ElapsedMilliseconds;
        }

        protected virtual INamespaceManager CreateManager()
        {
            var ns = new XmlNamespaceManager(new NameTable());

            return new BaseNamespaceManager(ns);
        }
    }
}