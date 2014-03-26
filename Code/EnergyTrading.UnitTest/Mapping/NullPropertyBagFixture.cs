namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class NullPropertyBagFixture : Fixture
    {
        [Test]
        public void NotPresentPropertyReturnsFalse()
        {
            var bag = new NullPropertyBag();

            Assert.IsFalse(bag["Profit"]);
        }

        [Test]
        public void AssignedPropertyReportsAsTrue()
        {
            var bag = new NullPropertyBag();
            bag["Profit"] = true;

            Assert.IsTrue(bag["Profit"]);            
        }

        [Test]
        public void AssignedPropertyClearedReportsAsFalse()
        {
            var bag = new NullPropertyBag();
            bag["Profit"] = true;
            bag["Profit"] = false;

            Assert.IsFalse(bag["Profit"]);
        }

        [Test]
        public void WhenLoadingAssignedIsIgnored()
        {
            var bag = new NullPropertyBag();
            bag.Loading = true;
            bag["Profit"] = true;

            bag.Assigned("Profit");

            Assert.IsTrue(bag["Profit"]); 
        }

        [Test]
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