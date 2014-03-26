namespace EnergyTrading.UnitTest
{
    using System;

    using EnergyTrading.Logging;

    using Moq;

    public class AssemblyLoggerProvider
    {
        private static Func<ILoggerFactory> provider;

        public static Mock<ILogger> MockLogger { get; private set; }

        public static void InitializeLogger()
        {
            MockLogger = new Mock<ILogger>();

            provider = LoggerFactory.GetProvider();
            var lm = new SimpleLoggerFactory(MockLogger.Object);
            
            LoggerFactory.SetProvider(() => lm);
        }

        public static void RestoreLogger()
        {
            LoggerFactory.SetProvider(provider);
        }
    }
}