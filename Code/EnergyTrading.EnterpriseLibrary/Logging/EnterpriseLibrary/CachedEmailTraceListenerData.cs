namespace EnergyTrading.Logging.EnterpriseLibrary
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq.Expressions;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    /// <summary>
    /// Represents the configuration settings that describe a <see cref="CachedEmailTraceListenerData"/>.
    /// </summary>
    public class CachedEmailTraceListenerData : EmailTraceListenerData
    {
        private const string EmailCacheDurationProperty = "emailCacheDuration";

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/>.
        /// </summary>
        public CachedEmailTraceListenerData()
        {
            this.ListenerDataType = typeof(CachedEmailTraceListenerData);
            this.Type = typeof(CachedEmailTraceListener);
        }

        /// <summary>
        /// Initializes a <see cref="CachedEmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// Default value for the SMTP port is 25
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        public CachedEmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, string formatterName)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatterName)
        {
        }

        /// <summary>
        /// Initializes a <see cref="CachedEmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        public CachedEmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
            : this("unnamed", toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName)
        {
        }

        /// <summary>
        /// Initializes a <see cref="CachedEmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        public CachedEmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
            : this(name, toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a <see cref="CachedEmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        ///<param name="traceOutputOptions">The trace options.</param>
        public CachedEmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions)
            : base(name, toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName, traceOutputOptions)
        {
            this.ListenerDataType = typeof(CachedEmailTraceListenerData);
            this.Type = typeof(CachedEmailTraceListener);
        }

        /// <summary>
        /// Initializes a <see cref="CachedEmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        public CachedEmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName, traceOutputOptions, filter)
        {
            this.ListenerDataType = typeof(CachedEmailTraceListenerData);
            this.Type = typeof(CachedEmailTraceListener);
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name, trace options
        /// and authentication information.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="authenticationMode">Authenticate mode to use.</param>
        /// <param name="userName">User name to pass to the server if using <see cref="EmailAuthenticationMode.UserNameAndPassword"/>.</param>
        /// <param name="password">Password to pass to the server if using <see cref="EmailAuthenticationMode.UserNameAndPassword"/>.</param>
        /// <param name="useSSL">Connect to the server using SSL?</param>
        public CachedEmailTraceListenerData(string name, 
            string toAddress, string fromAddress, 
            string subjectLineStarter, string subjectLineEnder, 
            string smtpServer, int smtpPort, 
            string formatterName, TraceOptions traceOutputOptions, SourceLevels filter,
            EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL)
            : base(name, 
            toAddress, fromAddress, 
            subjectLineStarter, subjectLineEnder, 
            smtpServer, smtpPort, 
            formatterName, traceOutputOptions, filter,
            authenticationMode, userName, password, useSSL)
        {
            this.ListenerDataType = typeof(CachedEmailTraceListenerData);
            this.Type = typeof(CachedEmailTraceListener);
        }

        [ConfigurationProperty(EmailCacheDurationProperty)]
        public double EmailCacheDuration
        {
            get { return (double)base[EmailCacheDurationProperty]; }
            set { base[EmailCacheDurationProperty] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                    new CachedEmailTraceListener(
                        this.ToAddress,
                        this.FromAddress,
                        this.SubjectLineStarter,
                        this.SubjectLineEnder,
                        this.SmtpServer,
                        this.SmtpPort,
                        Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter),
                        this.AuthenticationMode,
                        this.UserName,
                        this.Password,
                        this.UseSSL,
                        this.EmailCacheDuration);
        }
    }
}
