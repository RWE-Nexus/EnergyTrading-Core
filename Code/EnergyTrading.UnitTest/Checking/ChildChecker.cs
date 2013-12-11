namespace EnergyTrading.UnitTest.Checking
{
    using EnergyTrading.Test;

    public class ChildChecker : Checker<Child>
    {
        public ChildChecker()
        {
            Compare(x => x.Id);
            Compare(x => x.Name);
            Compare(x => x.Parent).Id();
        }
    }
}