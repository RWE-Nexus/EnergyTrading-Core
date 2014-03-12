namespace EnergyTrading.ServiceModel.Channels
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Endpoint behaviour that logs client messages.
    /// </summary>
    public class ClientMessageLoggerBehavior : IEndpointBehavior
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public IMessageLogger Logger { get; set; }

        /// <inheritdoc />
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <inheritdoc />
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new ClientMessageLogger { Logger = Logger };
            clientRuntime.MessageInspectors.Add(inspector);
        }

        /// <inheritdoc />
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <inheritdoc />
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}