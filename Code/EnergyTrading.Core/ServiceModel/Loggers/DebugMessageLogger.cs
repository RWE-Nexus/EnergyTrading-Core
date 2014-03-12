namespace EnergyTrading.ServiceModel.Loggers
{
    using System.Diagnostics;

    using EnergyTrading.ServiceModel;

    public class DebugMessageLogger : IMessageLogger
    {
        public string Path { get; set; }

        public void Sent(string message)
        {
            Debug.WriteLine("Sent: {0}", message);
        }

        public void Received(string message)
        {
            Debug.WriteLine("Received: {0}", message);
        }
    }
}