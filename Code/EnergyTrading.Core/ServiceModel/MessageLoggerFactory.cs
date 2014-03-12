namespace EnergyTrading.ServiceModel
{
    using EnergyTrading.Extensions;
    using EnergyTrading.ServiceModel.Loggers;

    /// <summary>
    /// Factory to create <see cref="IMessageLogger" /> instances.
    /// </summary>
    public static class MessageLoggerFactory
    {
        /// <summary>
        /// Create a logger
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IMessageLogger Create(string loggerName, string path)
        {
            var logger = CreateLogger(loggerName);
            logger.Path = path;

            return logger;
        }

        private static IMessageLogger CreateLogger(string logger)
        {
            if (string.IsNullOrEmpty(logger))
            {
                return new NullMessageLogger();
            }

            switch (logger.ToLowerInvariant())
            {
                case "null":
                    return new NullMessageLogger();
                case "console":
                    return new ConsoleMessageLogger();
                case "debug":
                    return new DebugMessageLogger();
                case "file":
                    return new FileMessageLogger();
                case "logger":
                    return new LoggingMessageLogger();
                default:
                    return logger.CreateInstance<IMessageLogger>();
            }
        }
    }
}