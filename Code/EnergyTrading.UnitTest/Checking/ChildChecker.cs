namespace EnergyTrading.UnitTest.Checking
{
    using EnergyTrading.Test;

    public class ChildChecker : Checker<Child>
    {
        public ChildChecker()
        {
            this.Compare(x => x.Id);
            this.Compare(x => x.Name);
            this.Compare(x => x.Parent).Id();
        }
    }
}