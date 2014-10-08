﻿namespace EnergyTrading.Logging.Log4Net
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Runtime.Caching;
    using System.Threading;

    using EnergyTrading.Caching;

    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;

    public class CachedSmtpAppender : SmtpAppender
    {
        private readonly MemoryCache cache = new MemoryCache("CachedSmtpAppender");
        private ICacheItemPolicyFactory cacheItemPolicyFactory = new AbsoluteCacheItemPolicyFactory(1);
        private double emailCacheDuration;
        private long cacheKey;

        public double EmailCacheDuration
        {
            set { this.emailCacheDuration = value; }
        }

        public ILayout SubjectLayout
        {
            get; set;
        }

        public override void ActivateOptions()
        {
            // log4net buffering always disabled - we make our own sending decisions
            this.BufferSize = 1;
            
            if (this.emailCacheDuration > 0)
            {
                // Email cache duration is in hours so convert it to seconds
                this.cacheItemPolicyFactory = new AbsoluteCacheItemPolicyFactory(Convert.ToInt32(this.emailCacheDuration * 3600));
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var previousOccurrences = this.GetPreviousOccurrences(this.cache, loggingEvent);
            
            var loggingEventCacheItem = this.AddNewCacheItem(loggingEvent);

            if (!previousOccurrences.Any(x => x.IsSent))
            {
                this.SetRepeatMessage(loggingEvent, previousOccurrences);

                this.SetSubject(loggingEvent);

                loggingEventCacheItem.IsSent = true;
                base.Append(loggingEvent);
            }
        }

        private IList<LoggingEventCacheItem> GetPreviousOccurrences(MemoryCache cache, LoggingEvent loggingEvent)
        {
            return cache.Select(x => x.Value)
                .Cast<LoggingEventCacheItem>()
                .Where(e =>
                    e.LoggingEvent.RenderedMessage == loggingEvent.RenderedMessage
                    && e.LoggingEvent.Level == loggingEvent.Level)
                .ToList();
        }

        private LoggingEventCacheItem AddNewCacheItem(LoggingEvent loggingEvent)
        {
            var loggingEventCacheItem = new LoggingEventCacheItem { LoggingEvent = loggingEvent };
            this.cache.Add(
                Interlocked.Increment(ref this.cacheKey).ToString(CultureInfo.InvariantCulture),
                loggingEventCacheItem,
                this.cacheItemPolicyFactory.CreatePolicy());
            return loggingEventCacheItem;
        }

        private void SetRepeatMessage(LoggingEvent loggingEvent, IList<LoggingEventCacheItem> cachedOccurrences)
        {
            var repeatMessage = string.Empty;
            if (cachedOccurrences.Any())
            {
                repeatMessage = string.Format(
                    "Number of occurrences in {0} hour(s): {1}{2}{2}",
                    this.emailCacheDuration,
                    cachedOccurrences.Count,
                    Environment.NewLine);
            }

            loggingEvent.Properties["repeats"] = repeatMessage;
        }

        private void SetSubject(LoggingEvent loggingEvent)
        {
            if (this.SubjectLayout == null)
            {
                return;
            }

            var subjectWriter = new StringWriter(CultureInfo.InvariantCulture);
            this.SubjectLayout.Format(subjectWriter, loggingEvent);
            this.Subject = subjectWriter.ToString();
        }

        public bool IsBodyHtml { get; set; }

        protected override void SendEmail(string messageBody)
        {
            using (var smtpClient = new SmtpClient())
            {
                if (!String.IsNullOrEmpty(SmtpHost))
                {
                    smtpClient.Host = SmtpHost;
                }
                smtpClient.Port = Port;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = EnableSsl;

                if (Authentication == SmtpAuthentication.Basic)
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
                }
                else if (Authentication == SmtpAuthentication.Ntlm)
                {
                    smtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                }

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.IsBodyHtml = IsBodyHtml;
                    mailMessage.Body = messageBody;
                    mailMessage.From = new MailAddress(From);
                    mailMessage.To.Add(To);
                    if (!String.IsNullOrEmpty(Cc))
                    {
                        mailMessage.CC.Add(Cc);
                    }
                    if (!String.IsNullOrEmpty(Bcc))
                    {
                        mailMessage.Bcc.Add(Bcc);
                    }
                    if (!String.IsNullOrEmpty(ReplyTo))
                    {
                        mailMessage.ReplyToList.Add(new MailAddress(ReplyTo));
                    }
                    mailMessage.Subject = Subject;
                    mailMessage.Priority = Priority;

                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}