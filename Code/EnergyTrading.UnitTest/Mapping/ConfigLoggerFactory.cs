namespace EnergyTrading.UnitTest.Mapping
{
    using NUnit.Framework;

    /// <summary>
    /// This is needed so that we get the AssemblyLoggerProvider code executed for this namespace.
    /// </summary>
    public class ConfigLoggerFactory
    {
        public void SetUp()
        {
            AssemblyLoggerProvider.InitializeLogger(); 
        }

        public void TearDown()
        {
            AssemblyLoggerProvider.RestoreLogger();
        }
    }
}