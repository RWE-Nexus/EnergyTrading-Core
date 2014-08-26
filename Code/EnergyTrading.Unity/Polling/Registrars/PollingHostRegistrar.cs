// -----------------------------------------------------------------------
// <copyright file="PollingHostRegistrar.cs" company="RWE">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace EnergyTrading.Polling.Registrars
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;
    using EnergyTrading.Container.Unity;
    using EnergyTrading.Logging;
    using EnergyTrading.Polling;
    using EnergyTrading.Polling.Configuration;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PollingHostRegistrar : IContainerRegistrar
    {
        private readonly ILogger logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PollingHostRegistrar()
        {
            this.SectionName = "pollingHost";
        }

        public string SectionName { get; set; }

        public void Register(IUnityContainer container)
        {
            // Load configuration section
            var section = ConfigurationManager.GetSection(this.SectionName) as PollingHostSection;
            if (section == null)
            {
                throw new NotSupportedException(string.Format("Must define pollingHost section named '{0}'", this.SectionName));
            }

            // Register the processors
            var p = new List<PollProcessorEndpoint>();
            foreach (var endpoint in section.PollProcessors.ToEndpoints())
            {
                p.AddRange(this.RegisterEndPoint(container, endpoint));
            }

            // Register the host under its interface
            container.RegisterType<IPollingHost, PollingHost>();
        }

        private IEnumerable<PollProcessorEndpoint> RegisterEndPoint(IUnityContainer container,
            PollProcessorEndpoint endpoint)
        {
            if (endpoint.SinglePolling && endpoint.Workers > 1)
            {
                logger.WarnFormat("{0} - Cannot have multiple workers for a Single Polling. Changing workers to 1",
                    endpoint.Name);
                endpoint.Workers = 1;
            }

            for (var index = 1; index < endpoint.Workers; index++)
            {
                var newEndPoint = new PollProcessorEndpoint
                {
                    Workers = 1,
                    SinglePolling = true,
                    Handler = endpoint.Handler,
                    IntervalSecs = endpoint.IntervalSecs,
                    Name = string.Format("{0}_{1}", endpoint.Name, index)
                };
                container.RegisterPollProcessor(newEndPoint);
                yield return newEndPoint;
            }

            endpoint.Workers = 1;

            container.RegisterPollProcessor(endpoint);
            yield return endpoint;
        }
    }
}
