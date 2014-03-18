namespace EnergyTrading.Contracts.Search
{
    using NUnit.Framework;

    [TestFixture]
    public class CriteriaFixture
    {
        [Test]
        public void ToStringForStringValue()
        {
            var x = new Criteria { Field = "x", Condition = SearchCondition.Equals, ComparisonValue = "fred" };

            Assert.AreEqual("x = \"fred\"", x.ToString());
        }

        [Test]
        public void ToStringForNumericValue()
        {
            var x = new Criteria { Field = "x", Condition = SearchCondition.Equals, ComparisonValue = "5", IsNumeric = true };

            Assert.AreEqual("x = 5", x.ToString());
        }

        [Test]
        public void DefaultHashCode()
        {
            var x = new Criteria();

            var y = x.GetHashCode();
        }

        [Test]
        public void NonDefaultHashCode()
        {
            var x = new Criteria { Field = "x", Condition = SearchCondition.Equals, ComparisonValue = "5", IsNumeric = true };

            var y = x.GetHashCode();
        }
    }
}