namespace RWEST.Nexus.UnitTest.SimpleData
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using global::Nexus.Data.SimpleData;

    using Simple.Data;

    [TestClass]
    public class InMemoryAdapterWithProceduresFixture
    {
        [TestMethod]
        public void CanCallProcedureAndSimulateOutputParameters()
        {
            var mock = new InMemoryAdapterWithProcedures();
            mock.AddProcedure("MyProcedure", p => { p.Add("output", "test"); return p; });
            Database.UseMockAdapter(mock);
            var db = Database.Open();
            dynamic candidate = db.MyProcedure("input");
            Assert.AreEqual("test", candidate.OutputValues["output"]);
        }

        [TestMethod]
        public void CanStillCallFunctions()
        {
            var mock = new InMemoryAdapterWithProcedures();
            mock.AddProcedure("MyProcedure", p => { p.Add("output", "test"); return p; });
            mock.AddFunction("Test", () => new[] { new Dictionary<string, object> { { "Foo", "Bar" } } });
            Database.UseMockAdapter(mock);
            var db = Database.Open();
            foreach (var row in db.Test())
            {
                Assert.AreEqual("Bar", row.Foo);
            }
        }
    }
}