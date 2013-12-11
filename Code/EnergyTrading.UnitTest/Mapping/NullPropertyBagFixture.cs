namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class NullPropertyBagFixture : Fixture
    {
        [TestMethod]
        public void NotPresentPropertyReturnsFalse()
        {
            var bag = new NullPropertyBag();

            Assert.IsFalse(bag["Profit"]);
        }

        [TestMethod]
        public void AssignedPropertyReportsAsTrue()
        {
            var bag = new NullPropertyBag();
            bag["Profit"] = true;

            Assert.IsTrue(bag["Profit"]);            
        }

        [TestMethod]
        public void AssignedPropertyClearedReportsAsFalse()
        {
            var bag = new NullPropertyBag();
            bag["Profit"] = true;
            bag["Profit"] = false;

            Assert.IsFalse(bag["Profit"]);
        }

        [TestMethod]
        public void WhenLoadingAssignedIsIgnored()
        {
            var bag = new NullPropertyBag();
            bag.Loading = true;
            bag["Profit"] = true;

            bag.Assigned("Profit");

            Assert.IsTrue(bag["Profit"]); 
        }

        [TestMethod]
        public void WhenNotLoadingAssignedClears()
        {
            var bag = new NullPropertyBag();
            bag.Loading = false;
            bag["Profit"] = true;

            bag.Assigned("Profit");

            Assert.IsFalse(bag["Profit"]);
        }
    }
}