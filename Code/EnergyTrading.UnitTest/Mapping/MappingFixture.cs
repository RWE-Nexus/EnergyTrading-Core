namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Test;

    using CheckerFactory = EnergyTrading.UnitTest.CheckerFactory;

    public abstract class MappingFixture<T> : Test.Mapping.MappingFixture<T>
        where T : class, new()
    {
        [TestMethod]
        public override void ShouldMapToDto()
        {
            base.ShouldMapToDto();
        }

        [TestMethod]
        public override void ShouldMapToXml()
        {
            base.ShouldMapToXml();
        }

        protected override ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }
    }
}