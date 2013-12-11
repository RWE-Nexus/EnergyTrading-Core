namespace EnergyTrading.UnitTest.Checking
{
    using EnergyTrading.Test;

    public class ParentChecker : Checker<Parent>
    {
        public ParentChecker()
        {
            Initialize();
            Exclude(x => x.Another);
        }
    }
}