namespace EnergyTrading.UnitTest
{
    using EnergyTrading.Logging;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class AssemblyLoggerProvider
    {
        public static Mock<ILogger> MockLogger { get; private set; }

        [TestFixtureSetUp]
        public static void AssemblyInitialize(TestContext context)
        {
            MockLogger = new Mock<ILogger>();
            var lm = new SimpleLoggerFactory(MockLogger.Object);
            LoggerFactory.SetProvider(() => lm);
        }
    }
}
