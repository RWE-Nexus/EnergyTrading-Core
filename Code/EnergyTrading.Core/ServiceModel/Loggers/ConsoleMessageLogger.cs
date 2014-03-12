namespace EnergyTrading.ServiceModel.Loggers
{
    using System;

    using EnergyTrading.ServiceModel;

    public class ConsoleMessageLogger : IMessageLogger
    {
        public string Path { get; set; }

        public void Sent(string message)
        {
            Console.WriteLine("Sent: {0}", message);
        }

        public void Received(string message)
        {
            Console.WriteLine("Received: {0}", message);
        }
    }
}