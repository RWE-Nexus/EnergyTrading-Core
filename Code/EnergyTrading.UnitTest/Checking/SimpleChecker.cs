namespace EnergyTrading.UnitTest.Checking
{
    using EnergyTrading.Test;

    public class SimpleChecker : Checker<Simple>
    {
        public SimpleChecker()
        {
            Compare(x => x.Id);
            Compare(x => x.Name);
        }
    }
}