namespace EnergyTrading.ServiceModel.Loggers
{
    using EnergyTrading.ServiceModel;

    /// <summary>
    /// A message logger that does nothing
    /// </summary>
    public class NullMessageLogger : IMessageLogger
    {
        /// <copydocfrom cref="IMessageLogger.Path" />
        public string Path { get; set; }

        /// <copydocfrom cref="IMessageLogger.Sent" />
        public void Sent(string message)
        {
        }

        /// <copydocfrom cref="IMessageLogger.Received" />
        public void Received(string message)
        {
        }
    }
}