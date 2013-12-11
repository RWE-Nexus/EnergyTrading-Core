namespace EnergyTrading.Logging.Log4Net
{
    using log4net.Core;

    internal class LoggingEventCacheItem
    {
        public LoggingEvent LoggingEvent { get; set; }

        public bool IsSent { get; set; }
    }
}