namespace EnergyTrading.Logging.EnterpriseLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading;

    using EnergyTrading.Caching;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    [ConfigurationElementType(typeof(CachedEmailTraceListenerData))]
    public class CachedEmailTraceListener : EmailTraceListener
    {
        private readonly ICacheItemPolicyFactory cacheItemPolicyFactory = new AbsoluteCacheItemPolicyFactory(1);
        private readonly MemoryCache debugCache = new MemoryCache("CachedEmailTraceListener.Debug");
        private readonly MemoryCache errorCache = new MemoryCache("CachedEmailTraceListener.Error");
        private readonly MemoryCache warningCache = new MemoryCache("CachedEmailTraceListener.Warn");
        private readonly MemoryCache infoCache = new MemoryCache("CachedEmailTraceListener.Info");
        private double emailCacheDuration = 0;

        // To get unique key to add to Memory Cache
        private long debugCount = 0;
        private long errorCount = 0;
        private long warnCount = 0;
        private long infoCount = 0;

        public CachedEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, ILogFormatter formatter)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, formatter)
        {
        }

        public CachedEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, ILogFormatter formatter)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatter)
        {
        }

        public CachedEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, ILogFormatter formatter, EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL, double emailCacheDuration)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatter, authenticationMode, userName, password, useSSL)
        {
            this.emailCacheDuration = emailCacheDuration;
            if (emailCacheDuration > 0)
            {
                // Email cache duration is in hours
                this.cacheItemPolicyFactory = new AbsoluteCacheItemPolicyFactory(Convert.ToInt32(emailCacheDuration * 3600));
            }
        }

        public CachedEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer)
        {
        }

        public CachedEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort)
        {
        }

        public CachedEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, authenticationMode, userName, password, useSSL)
        {
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                var logEntry = data as LogEntry;
                var currentDate = DateTime.Now;
                EmailLogEntry emailLogEntry;
                IList<EmailLogEntry> previousOccurrences = new List<EmailLogEntry>();

                if (logEntry != null)
                {
                    emailLogEntry = new EmailLogEntry(logEntry);
                    switch (logEntry.Severity)
                    {
                        case TraceEventType.Verbose:
                            previousOccurrences = this.GetPreviousOccurrences(this.debugCache, logEntry);
                            this.debugCache.Add(
                                Interlocked.Increment(ref debugCount).ToString(CultureInfo.InvariantCulture),
                                emailLogEntry,
                                cacheItemPolicyFactory.CreatePolicy());
                            break;

                        case TraceEventType.Warning:
                            previousOccurrences = this.GetPreviousOccurrences(this.warningCache, logEntry);
                            this.warningCache.Add(
                                Interlocked.Increment(ref warnCount).ToString(CultureInfo.InvariantCulture),
                                emailLogEntry,
                                cacheItemPolicyFactory.CreatePolicy());
                            break;

                        case TraceEventType.Error:
                        case TraceEventType.Critical:
                            previousOccurrences = this.GetPreviousOccurrences(this.errorCache, logEntry);
                            this.errorCache.Add(
                                Interlocked.Increment(ref errorCount).ToString(CultureInfo.InvariantCulture),
                                emailLogEntry,
                                cacheItemPolicyFactory.CreatePolicy());
                            break;

                        case TraceEventType.Information:
                            previousOccurrences = this.GetPreviousOccurrences(this.infoCache, logEntry);
                            this.infoCache.Add(
                                Interlocked.Increment(ref infoCount).ToString(CultureInfo.InvariantCulture),
                                emailLogEntry,
                                cacheItemPolicyFactory.CreatePolicy());
                            break;
                    }

                    if (previousOccurrences.Any())
                    {
                        if (previousOccurrences.Any(x=> x.HasSendInEmail))
                        {
                            return;
                        }

                        logEntry.ExtendedProperties.Add("Number of occurrences in "+this.emailCacheDuration +" hour(s)", previousOccurrences.Count);
                    }

                    emailLogEntry.HasSendInEmail = true;
                    emailLogEntry.EmailSendOn = currentDate;
                }

                base.TraceData(eventCache, source, eventType, id, data);
            }
        }

        private IList<EmailLogEntry> GetPreviousOccurrences(MemoryCache cache, LogEntry logEntry)
        {
            return cache.Select(x => x.Value).Cast<EmailLogEntry>()
                .Where(e => e.LogEntry.Message == logEntry.Message)
                .ToList();
        }
    }    
}