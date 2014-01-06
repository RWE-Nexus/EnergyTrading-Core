namespace EnergyTrading.Logging.EnterpriseLibrary
{
    using System;

    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class EmailLogEntry
    {
        public LogEntry LogEntry { get; set; }

        public EmailLogEntry(LogEntry logEntry)
        {
            this.LogEntry = logEntry;
        }

        public bool HasSendInEmail { get; set; }

        public DateTime EmailSendOn { get; set; }
    }
}