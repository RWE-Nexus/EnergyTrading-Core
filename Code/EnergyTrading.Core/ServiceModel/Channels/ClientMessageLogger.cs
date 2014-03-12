namespace EnergyTrading.ServiceModel.Channels
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using EnergyTrading.ServiceModel.Loggers;

    /// <summary>
    /// Client Message inspector that logs sent/received messages.
    /// </summary>
    public class ClientMessageLogger : IClientMessageInspector
    {
        private IMessageLogger logger;

        /// <summary>
        /// Get or set the logger.
        /// </summary>
        public IMessageLogger Logger
        {
            get { return logger ?? (logger = new ConsoleMessageLogger()); }
            set { logger = value; }
        }

        /// <copydocfrom cref="IClientMessageInspector.AfterReceiveReply" />
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            reply = LogMessage(reply, Logger.Received);
        }

        /// <copydocfrom cref="IClientMessageInspector.BeforeSendRequest" />
        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            request = LogMessage(request, Logger.Received);

            return null;
        }

        private Message LogMessage(Message original, Action<string> log)
        {
            // Need to create a copy as if a stream we can't transmit once we serialize
            var buffer = original.CreateBufferedCopy(int.MaxValue);
            log(buffer.MessageContent());

            return buffer.CreateMessage();
        }
    }
}