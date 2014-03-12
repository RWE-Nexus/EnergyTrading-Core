namespace EnergyTrading.ServiceModel.Loggers
{
    using System.Reflection;

    using EnergyTrading.Logging;
    using EnergyTrading.ServiceModel;

    public class LoggingMessageLogger : IMessageLogger
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Path { get; set; }

        public void Sent(string message)
        {
            Logger.Info("Sent: " + message);
        }

        public void Received(string message)
        {
            Logger.Info("Received: " + message);
        }
    }
}