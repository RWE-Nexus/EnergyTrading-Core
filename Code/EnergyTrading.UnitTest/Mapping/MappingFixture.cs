namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Test;

    using NUnit.Framework;

    using CheckerFactory = EnergyTrading.UnitTest.CheckerFactory;

    public abstract class MappingFixture<T> : EnergyTrading.Test.Mapping.MappingFixture<T>
        where T : class, new()
    {
        [Test]
        public override void ShouldMapToDto()
        {
            base.ShouldMapToDto();
        }

        [Test]
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