namespace EnergyTrading.UnitTest.Mapping
{
    using NUnit.Framework;

    /// <summary>
    /// This is needed so that we get the AssemblyLoggerProvider code executed for this namespace.
    /// </summary>
    [SetUpFixture]
    public class ConfigLoggerFactory
    {
        [SetUp]
        public void SetUp()
        {
            AssemblyLoggerProvider.InitializeLogger(); 
        }

        [TearDown]
        public void TearDown()
        {
            AssemblyLoggerProvider.RestoreLogger();
        }
    }
}