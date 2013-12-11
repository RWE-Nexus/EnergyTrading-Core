namespace EnergyTrading.FileProcessing.Registrars
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.FileProcessing;
    using EnergyTrading.FileProcessing.Configuration;

    /// <summary>
    /// Registers the file processor host and associated file processors
    /// </summary>
    public class FileProcessorHostRegistrar : IContainerRegistrar
    {
        public FileProcessorHostRegistrar()
        {
            this.SectionName = "fileProcessorHost";
        }

        public string SectionName { get; set; }

        public void Register(IUnityContainer container)
        {
            // Load configuration section
            var section = ConfigurationManager.GetSection(this.SectionName) as FileProcessorHostSection;
            if (section == null)
            {
                throw new NotSupportedException(string.Format("Must define fileProcessorHost section named '{0}'", this.SectionName));
            }

            // Register the processors
            var p = new List<FileProcessorEndpoint>();
            foreach (var endpoint in section.Processors.ToEndpoints())
            {
                container.RegisterProcessor(endpoint);
                p.Add(endpoint);
            }

            // Register the host under its interface
            container.RegisterType<IFileProcessorHost, FileProcessorHost>();
        }
    }
}