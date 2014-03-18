namespace EnergyTrading.UnitTest
{
    using EnergyTrading.Logging;

    using Moq;

    public class AssemblyLoggerProvider
    {
        public static Mock<ILogger> MockLogger { get; private set; }

        public static void InitializeLogger()
        {
            MockLogger = new Mock<ILogger>();
            var lm = new SimpleLoggerFactory(MockLogger.Object);
            LoggerFactory.SetProvider(() => lm);
        }
    }
}