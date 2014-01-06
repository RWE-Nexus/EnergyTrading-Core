namespace EnergyTrading.UnitTest
{
    using EnergyTrading.Logging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

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
