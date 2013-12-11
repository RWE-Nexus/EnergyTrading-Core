using System.Collections.Specialized;

namespace EnergyTrading.Logging.Log4Net
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

    using log4net.Config;

    using EnergyTrading.Configuration;

    public class Log4NetConfiguration : IConfigurationTask
    {
        public Log4NetConfiguration()
        {
            try
            {
                var section = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Sections["log4net"];
                if (section != null)
                {
                    var configSource = section.SectionInformation.ConfigSource;
                    LoggingFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configSource);
                }
                else
                {
                    LoggingFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logger.config");
                }
            }
            catch (Exception)
            {
                //For the TEST projects it is not a condition to have log4net config file included in the project. Not doing any thing. 
                //Cannot log as logger configuration are not yet loaded
            }
        }

        public IList<Type> DependsOn
        {
            get { return new Type[] { }; }
        }

        public string LoggingFile { get; set; }

        /// <summary>
        /// Configures log4net, either from a logger.config file or the app.config
        /// </summary>
        public void Configure()
        {
            if (File.Exists(LoggingFile))
            {
                var fi = new FileInfo(LoggingFile);
                XmlConfigurator.ConfigureAndWatch(fi);

                return;
            }

            if (ConfigurationManager.GetSection("log4net") != null)
            {
                XmlConfigurator.Configure();
            }
        }
    }
}