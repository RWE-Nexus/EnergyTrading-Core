namespace EnergyTrading.UnitTest.Checking
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Test;
    using EnergyTrading.Test.Checking;
    using EnergyTrading.Test.Data;

    [TestClass]
    public class CheckerBuilderFixture : Fixture
    {
        protected override ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }

        [TestMethod]
        public void BuildSimplePropertyChecker()
        {
            var builder = new CheckerBuilder(CheckerFactory);

            var checker = (ICheckerCompare) builder.Build(typeof(Fred));

            Assert.AreEqual(1, checker.Properties.Count, "Property count differs");
        }

        [TestMethod]
        public void BuildDescendantPropertyChecker()
        {
            var builder = new CheckerBuilder(CheckerFactory);

            var checker = (ICheckerCompare)builder.Build(typeof(Jim));

            Assert.AreEqual(1, checker.Properties.Count, "Property count differs");
        }

        protected override void OnSetup()
        {
            base.OnSetup();

            PropertyCheck.IdentityChecker = new IdentifiableChecker();
            PropertyCheck.Targeter = new TypeCompareTargeter();
        }

        private class Fred
        {
            public int Id { get; set; }
        }

        private class Jim : Fred
        {
            public string Name { get; set; }
        }
    }
}