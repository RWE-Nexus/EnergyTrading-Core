namespace EnergyTrading.UnitTest.Checking
{
    using EnergyTrading.Test;

    public class SimpleChecker : Checker<Simple>
    {
        public SimpleChecker()
        {
            this.Compare(x => x.Id);
            this.Compare(x => x.Name);
        }
    }
}