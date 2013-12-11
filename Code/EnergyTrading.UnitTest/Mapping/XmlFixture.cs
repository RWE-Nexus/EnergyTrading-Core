namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Test;

    using CheckerFactory = EnergyTrading.UnitTest.CheckerFactory;

    public class XmlFixture : EnergyTrading.Test.Xml.XmlFixture
    {
        protected override ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }
    }
}