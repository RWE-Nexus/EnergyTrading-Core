namespace EnergyTrading.Test
{
    /// <summary>
    /// Stub implementation of a checker builder that always returns null;
    /// </summary>
    public class NullCheckerBuilder : ICheckerBuilder
    {
        public IChecker Build(System.Type type)
        {
            return null;
        }
    }
}
