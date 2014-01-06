namespace EnergyTrading.UnitTest
{
    using EnergyTrading.Test;

    public class CheckerFactory : EnergyTrading.Test.CheckerFactory
    {
        public CheckerFactory()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.Register(typeof(CheckerFactory).Assembly);
            this.Register(typeof(EnergyTrading.Test.CheckerFactory).Assembly);

            this.Builder = new CheckerBuilder(this);
        }
    }
}