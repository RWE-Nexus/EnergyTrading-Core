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

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Polling;
    using EnergyTrading.Polling.Configuration;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PollingHostRegistrar : IContainerRegistrar
    {
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
                container.RegisterPollProcessor(endpoint);
                p.Add(endpoint);
            }

            // Register the host under its interface
            container.RegisterType<IPollingHost, PollingHost>();
        }
    }
}
