namespace EnergyTrading.Contracts.Search
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CriteriaFixture
    {
        [TestMethod]
        public void ToStringForStringValue()
        {
            var x = new Criteria { Field = "x", Condition = SearchCondition.Equals, ComparisonValue = "fred" };

            Assert.AreEqual("x = \"fred\"", x.ToString());
        }

        [TestMethod]
        public void ToStringForNumericValue()
        {
            var x = new Criteria { Field = "x", Condition = SearchCondition.Equals, ComparisonValue = "5", IsNumeric = true };

            Assert.AreEqual("x = 5", x.ToString());
        }

        [TestMethod]
        public void DefaultHashCode()
        {
            var x = new Criteria();

            var y = x.GetHashCode();
        }

        [TestMethod]
        public void NonDefaultHashCode()
        {
            var x = new Criteria { Field = "x", Condition = SearchCondition.Equals, ComparisonValue = "5", IsNumeric = true };

            var y = x.GetHashCode();
        }
    }
}