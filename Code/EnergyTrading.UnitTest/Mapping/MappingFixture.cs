namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Test;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using CheckerFactory = EnergyTrading.UnitTest.CheckerFactory;

    public abstract class MappingFixture<T> : EnergyTrading.Test.Mapping.MappingFixture<T>
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