namespace EnergyTrading.UnitTest.SimpleData
{
    using System.Collections.Generic;

    using EnergyTrading.Data.SimpleData;

    using NUnit.Framework;

    using Simple.Data;

    [TestFixture]
    public class InMemoryAdapterWithProceduresFixture
    {
        [Test]
        public void CanCallProcedureAndSimulateOutputParameters()
        {
            var mock = new InMemoryAdapterWithProcedures();
            mock.AddProcedure("MyProcedure", p => { p.Add("output", "test"); return p; });
            Database.UseMockAdapter(mock);
            var db = Database.Open();
            dynamic candidate = db.MyProcedure("input");
            Assert.AreEqual("test", candidate.OutputValues["output"]);
        }

        [Test]
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