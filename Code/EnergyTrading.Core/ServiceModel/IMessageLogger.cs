namespace EnergyTrading.ServiceModel
{
    /// <summary>
    /// Logs WCF messages.
    /// </summary>
    public interface IMessageLogger
    {
        /// <summary>
        /// Gets or sets the path to log to.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Log a sent message.
        /// </summary>
        /// <param name="message"></param>
        void Sent(string message);

        /// <summary>
        /// Log a received message.
        /// </summary>
        /// <param name="message"></param>
        void Received(string message);
    }
}