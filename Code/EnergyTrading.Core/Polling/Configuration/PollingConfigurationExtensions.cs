namespace EnergyTrading.Polling.Configuration
{
    using System.Collections.Generic;
    using System.Linq;

    using EnergyTrading.Extensions;

    public static class PollingConfigurationExtensions
    {
        public static IEnumerable<PollProcessorEndpoint> ToEndpoints(this IEnumerable<PollProcessorElement> collection)
        {
            return collection.Select(element => element.ToEndpoint());
        }

        public static PollProcessorEndpoint ToEndpoint(this PollProcessorElement element)
        {
            var endpoint = new PollProcessorEndpoint
            {
                Name = element.Name,
                IntervalSecs = element.IntervalSecs,
                Handler = element.Handler.ToType(),
                SinglePolling = element.SinglePolling,
                Workers = element.Workers
            };

            return endpoint;
        }
    }
}
