﻿namespace EnergyTrading.UnitTest.FileProcessing.Configuration
{
    using System;
    using System.IO;

    using EnergyTrading.FileProcessing;
    using EnergyTrading.FileProcessing.Configuration;

    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationExtensionsFixture : Fixture
    {
        [Test]
        public void ConvertProcessorElementToEndpointWithEmptyProcessorConfigurator()
        {
            var element = new FileProcessorElement
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                ScavengeInterval = 100,
                RecoveryInterval = 100,
                PollingInterval = 130
            };

            var expected = new FileProcessorEndpoint
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                NumberOfConsumers = 1,
                ScavengeInterval = new TimeSpan(0, 0, 100),
                RecoveryInterval = new TimeSpan(0, 0, 100),
                ProcessorConfigurator = "EnergyTrading.FileProcessing.Registrars.EventBasedProcessorDefaultRegistrar, EnergyTrading.Unity",
                AdditionalFilter = typeof(DefaultFileFilter),
                PollingRestartInterval = new TimeSpan(0, 0, 60),
                PollingInterval = 130000
            };

            var candidate = element.ToEndpoint();
            Check(expected, candidate);
        }

        [Test]
        public void ConvertProcessorElementToEndpointWithNtfsBasedProcessorConfigurator()
        {
            var element = new FileProcessorElement
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                ScavengeInterval = 100,
                RecoveryInterval = 100,
                ProcessorConfiguratorType = "EventBased"
            };

            var expected = new FileProcessorEndpoint
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                NumberOfConsumers = 1,
                ScavengeInterval = new TimeSpan(0, 0, 100),
                RecoveryInterval = new TimeSpan(0, 0, 100),
                ProcessorConfigurator = FileProcessorEndpoint.EventBasedConfiguratorType,
                AdditionalFilter = typeof(DefaultFileFilter),
                PollingRestartInterval = new TimeSpan(0, 0, 60),
                PollingInterval = 120000
            };

            var candidate = element.ToEndpoint();
            Check(expected, candidate);
        }

        [Test]
        public void ConvertProcessorElementToEndpointWithSambaBasedProcessor()
        {
            var element = new FileProcessorElement
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                ScavengeInterval = 100,
                RecoveryInterval = 100,
                ProcessorConfiguratorType = "PollingBased"
            };

            var expected = new FileProcessorEndpoint
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                NumberOfConsumers = 1,
                ScavengeInterval = new TimeSpan(0, 0, 100),
                RecoveryInterval = new TimeSpan(0, 0, 100),
                ProcessorConfigurator = FileProcessorEndpoint.PollingBasedConfiguratorType,
                AdditionalFilter = typeof(DefaultFileFilter),
                PollingRestartInterval = new TimeSpan(0, 0, 60),
                PollingInterval = 120000
            };

            var candidate = element.ToEndpoint();
            Check(expected, candidate);
        }

        [Test]
        public void ConvertProcessorElementWithCustomProcessor()
        {
            var element = new FileProcessorElement
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                ScavengeInterval = 100,
                RecoveryInterval = 100,
                ProcessorConfiguratorType = "EnergyTrading.UnitTest.FileProcessing.Configuration.ConfigurationExtensionsFixture+CustomFileProcessor, EnergyTrading.UnitTest",
                Handler = "EnergyTrading.UnitTest.FileProcessing.FileHandler, EnergyTrading.UnitTest"
            };

            var expected = new FileProcessorEndpoint
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                NumberOfConsumers = 1,
                ScavengeInterval = new TimeSpan(0, 0, 100),
                RecoveryInterval = new TimeSpan(0, 0, 100),
                ProcessorConfigurator = "EnergyTrading.UnitTest.FileProcessing.Configuration.ConfigurationExtensionsFixture+CustomFileProcessor, EnergyTrading.UnitTest",
                Handler = typeof(FileHandler),
                AdditionalFilter = typeof(DefaultFileFilter),
                PollingRestartInterval = new TimeSpan(0, 0, 60),
                PollingInterval = 120000
            };

            var candidate = element.ToEndpoint();
            Check(expected, candidate);
        }

        [Test]
        public void ConvertProcessorElementWithCustomPostProcessor()
        {
            var element = new FileProcessorElement
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                Consumers = 2,
                ScavengeInterval = 100,
                RecoveryInterval = 100,
                PollingInactivityRestartInterval = 100,
                Handler = "EnergyTrading.UnitTest.FileProcessing.FileHandler, EnergyTrading.UnitTest",
                PostProcessor = "EnergyTrading.UnitTest.FileProcessing.Configuration.ConfigurationExtensionsFixture+CustomPostProcessor, EnergyTrading.UnitTest"
            };

            var expected = new FileProcessorEndpoint
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                NumberOfConsumers = 2,
                ScavengeInterval = new TimeSpan(0, 0, 100),
                RecoveryInterval = new TimeSpan(0, 0, 100),
                ProcessorConfigurator = FileProcessorEndpoint.EventBasedConfiguratorType,
                Handler = typeof(FileHandler),
                PollingRestartInterval = new TimeSpan(0, 0, 100),
                AdditionalFilter = typeof(DefaultFileFilter),
                PostProcessor = typeof(CustomPostProcessor),
                PollingInterval = 120000
            };

            var candidate = element.ToEndpoint();
            Check(expected, candidate);
        }

        [Test]
        public void ConvertProcessorElementWithCustomFileFilter()
        {
            var element = new FileProcessorElement
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                Consumers = 2,
                ScavengeInterval = 100,
                RecoveryInterval = 100,
                PollingInactivityRestartInterval = 100,
                Handler = "EnergyTrading.UnitTest.FileProcessing.FileHandler, EnergyTrading.UnitTest",
                PostProcessor = "EnergyTrading.UnitTest.FileProcessing.Configuration.ConfigurationExtensionsFixture+CustomPostProcessor, EnergyTrading.UnitTest",
                AdditionalFilter = "EnergyTrading.UnitTest.FileProcessing.Configuration.ConfigurationExtensionsFixture+CustomFileFilter, EnergyTrading.UnitTest"
            };

            var expected = new FileProcessorEndpoint
            {
                Name = "a",
                DropPath = "b",
                Filter = "*",
                SuccessPath = "c",
                FailurePath = "d",
                InProgressPath = "e",
                NumberOfConsumers = 2,
                ScavengeInterval = new TimeSpan(0, 0, 100),
                RecoveryInterval = new TimeSpan(0, 0, 100),
                ProcessorConfigurator = FileProcessorEndpoint.EventBasedConfiguratorType,
                Handler = typeof(FileHandler),
                PollingRestartInterval = new TimeSpan(0, 0, 100),
                AdditionalFilter = typeof(CustomFileFilter),
                PostProcessor = typeof(CustomPostProcessor),
                PollingInterval = 120000
            };

            var candidate = element.ToEndpoint();
            Check(expected, candidate);
        }

        public class CustomFileFilter : IFileFilter
        {
            public bool IncludeFile(string fullFilePath)
            {
                throw new NotImplementedException();
            }
        }

        public class CustomPostProcessor : IFilePostProcessor
        {
            public void PostProcess(string outputFile, bool successful)
            {
            }
        }

        public class CustomHandlerAndPostProcessor : IFileHandler, IFilePostProcessor
        {
            public bool Handle(FileInfo fileInfo, string originalFileName)
            {
                return true;
            }

            public void PostProcess(string outputFile, bool successful)
            {
            }
        }

        public class CustomFileProcessor : FileProcessor
        {
            public CustomFileProcessor(FileProcessorEndpoint endpoint, IFileHandler handler, IFilePostProcessor postProcessor)
                : base(endpoint, handler, postProcessor)
            {
            }
        }
    }
}