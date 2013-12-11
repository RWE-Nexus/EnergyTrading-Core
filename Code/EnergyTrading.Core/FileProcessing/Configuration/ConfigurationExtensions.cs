namespace EnergyTrading.FileProcessing.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EnergyTrading.Extensions;

    public static class ConfigurationExtensions
    {
        public static IEnumerable<FileProcessorEndpoint> ToEndpoints(this IEnumerable<FileProcessorElement> collection)
        {
            return collection.Select(element => element.ToEndpoint());
        }

        public static FileProcessorEndpoint ToEndpoint(this FileProcessorElement element)
        {
            var endpoint = new FileProcessorEndpoint
            {
                Name = element.Name,
                DropPath = element.DropPath,
                InProgressPath = element.InProgressPath,
                Filter = element.Filter,
                MonitorSubdirectories = element.MonitorSubdirectories,
                ScavengeInterval = new TimeSpan(0, 0, element.ScavengeInterval),
                RecoveryInterval = new TimeSpan(0, 0, element.RecoveryInterval),
                PollingRestartInterval = new TimeSpan(0, 0, element.PollingInactivityRestartInterval),
                NumberOfConsumers = element.Consumers,
                SuccessPath = element.SuccessPath,
                FailurePath = element.FailurePath,
                ProcessorConfigurator = DetermineProcessorType(element),
                Handler = element.Handler.ToType(),
                PostProcessor = element.PostProcessor.ToType(),
                AdditionalFilter = element.AdditionalFilter.ToType() ?? typeof(DefaultFileFilter)
            };
            return endpoint;
        }

        private static string DetermineProcessorType(FileProcessorElement element)
        {
            switch (element.ProcessorConfiguratorType)
            {
                case null:
                case "EventBased":
                    return FileProcessorEndpoint.EventBasedConfiguratorType;
                case "PollingBased":
                    return FileProcessorEndpoint.PollingBasedConfiguratorType;
                default:
                    return element.ProcessorConfiguratorType;
            }
        }
    }
}
