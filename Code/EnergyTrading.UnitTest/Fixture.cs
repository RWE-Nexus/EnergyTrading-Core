namespace EnergyTrading.UnitTest
{
    using EnergyTrading.Test;

    public class Fixture : EnergyTrading.Test.Fixture
    {
        protected override ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }
    }
}