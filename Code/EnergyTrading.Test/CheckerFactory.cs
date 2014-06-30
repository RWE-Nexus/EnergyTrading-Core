namespace EnergyTrading.Test
{
    using NCheck.Checking;
    using NCheck.Test;

    /// <summary>
    /// A factory class that provides checking facilities for objects so that property level comparisons can be easily made
    /// </summary>
    public class CheckerFactory : NCheck.CheckerFactory, ICheckerFactory
    {
        public CheckerFactory()
        {
            PropertyCheck.IdentityChecker = new IdentifiableChecker();
        }
    }
}