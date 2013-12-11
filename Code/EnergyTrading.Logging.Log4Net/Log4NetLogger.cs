namespace EnergyTrading.Logging.Log4Net
{
    using System;
    using System.Globalization;

    using log4net;
    using log4net.Core;
    using log4net.Util;

    using ILogger = EnergyTrading.Logging.ILogger;

    /// <summary>
    /// A log4net implementation of ILogger.
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private static readonly Type LoggerType;
        private readonly ILog logger;

        static Log4NetLogger()
        {
            LoggerType = typeof(Log4NetLogger);
        }

        /// <summary>
        /// Constructor taking a log4net ILog instance to delegate log calls to.
        /// </summary>
        /// <param name="log">A log4net ILog instance.</param>
        public Log4NetLogger(ILog log)
        {            
            this.logger = log;
        }

        /// <inheritdoc />
        public bool IsDebugEnabled
        {
            get { return logger.IsDebugEnabled; }
        }

        /// <inheritdoc />
        public bool IsInfoEnabled
        {
            get { return logger.IsInfoEnabled; }
        }

        /// <inheritdoc />
        public bool IsWarnEnabled
        {
            get { return logger.IsWarnEnabled; }
        }

        /// <inheritdoc />
        public bool IsErrorEnabled
        {
            get { return logger.IsErrorEnabled; }
        }

        /// <inheritdoc />
        public bool IsFatalEnabled
        {
            get { return logger.IsFatalEnabled; }
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            this.logger.Logger.Log(LoggerType, Level.Debug, message, null);
        }

        /// <inheritdoc />
        public void Debug(string message, Exception exception)
        {
            this.logger.Logger.Log(LoggerType, Level.Debug, message, exception);
        }

        /// <inheritdoc />
        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                this.logger.Logger.Log(LoggerType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.logger.Logger.Log(LoggerType, Level.Info, message, null);
        }

        /// <inheritdoc />
        public void Info(string message, Exception exception)
        {
            this.logger.Logger.Log(LoggerType, Level.Info, message, exception);
        }

        /// <inheritdoc />
        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                this.logger.Logger.Log(LoggerType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <inheritdoc />
        public void Warn(string message)
        {
            this.logger.Logger.Log(LoggerType, Level.Warn, message, null);
        }

        /// <inheritdoc />
        public void Warn(string message, Exception exception)
        {
            this.logger.Logger.Log(LoggerType, Level.Warn, message, exception);
        }

        /// <inheritdoc />
        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                this.logger.Logger.Log(LoggerType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            this.logger.Logger.Log(LoggerType, Level.Error, message, null);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception)
        {
            this.logger.Logger.Log(LoggerType, Level.Error, message, exception);
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                this.logger.Logger.Log(LoggerType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <inheritdoc />
        public void Fatal(string message)
        {
            this.logger.Logger.Log(LoggerType, Level.Fatal, message, null);
        }

        /// <inheritdoc />
        public void Fatal(string message, Exception exception)
        {
            this.logger.Logger.Log(LoggerType, Level.Fatal, message, exception);
        }

        /// <inheritdoc />
        public void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                this.logger.Logger.Log(LoggerType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }
    }
}