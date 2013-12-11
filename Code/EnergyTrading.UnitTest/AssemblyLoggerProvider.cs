namespace EnergyTrading.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Logging;

    [TestClass]
    public class AssemblyLoggerProvider
    {
        public static Mock<ILogger> MockLogger { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            MockLogger = new Mock<ILogger>();
            var lm = new SimpleLoggerFactory(MockLogger.Object);
            LoggerFactory.SetProvider(() => lm);
        }
    }
}
