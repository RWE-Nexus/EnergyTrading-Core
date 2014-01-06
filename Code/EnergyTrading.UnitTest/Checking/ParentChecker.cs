namespace EnergyTrading.UnitTest.Checking
{
    using EnergyTrading.Test;

    public class ParentChecker : Checker<Parent>
    {
        public ParentChecker()
        {
            this.Initialize();
            this.Exclude(x => x.Another);
        }
    }
}