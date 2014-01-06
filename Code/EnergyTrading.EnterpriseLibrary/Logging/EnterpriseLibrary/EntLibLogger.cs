namespace EnergyTrading.Logging.EnterpriseLibrary
{
    using System;
    using System.Diagnostics;

    using EnergyTrading;
    using EnergyTrading.Logging;

    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class EntLibLogger : ILogger
    {
        private readonly LogWriter logWriter;

        public EntLibLogger(LogWriter logWriter)
        {
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            this.logWriter = logWriter;
        }

        /// <inheritdoc />
        public bool IsDebugEnabled
        {
            get { return logWriter.ShouldLog(new LogEntry { Severity = TraceEventType.Verbose }); }
        }

        /// <inheritdoc />
        public bool IsInfoEnabled
        {
            get { return logWriter.ShouldLog(new LogEntry { Severity = TraceEventType.Information }); }
        }

        /// <inheritdoc />
        public bool IsWarnEnabled
        {
            get { return logWriter.ShouldLog(new LogEntry { Severity = TraceEventType.Warning }); }
        }

        /// <inheritdoc />
        public bool IsErrorEnabled
        {
            get { return logWriter.ShouldLog(new LogEntry { Severity = TraceEventType.Error}); }
        }

        /// <inheritdoc />
        public bool IsFatalEnabled
        {
            get { return logWriter.ShouldLog(new LogEntry { Severity = TraceEventType.Critical }); }
        }

        public void Debug(string message)
        {
            this.Log("Debug", message);
        }

        public void Debug(string message, Exception exception)
        {
            this.Log("Debug", message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            this.LogFormat("Debug", format, args);
        }

        public void Info(string message)
        {
            this.Log("Info", message);
        }

        public void Info(string message, Exception exception)
        {
            this.Log("Info", message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.LogFormat("Info", format, args);
        }

        public void Warn(string message)
        {
            this.Log("Warn", message);
        }

        public void Warn(string message, Exception exception)
        {
            this.Log("Warn", message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.LogFormat("Warn", format, args);
        }

        public void Error(string message)
        {
            this.Log("Error", message);
        }

        public void Error(string message, Exception exception)
        {
            this.Log("Error", message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.LogFormat("Error", format, args);
        }

        public void Fatal(string message)
        {
            this.Log("Fatal", message);
        }

        public void Fatal(string message, Exception exception)
        {
            this.Log("Fatal", message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this.LogFormat("Fatal", format, args);
        }

        public void Log(string severity, string message, Exception exception)
        {
            var messageWithException = string.Format(
                "{0}{1}Exception: {2}{3}StackTrace: {4}",
                message,
                Environment.NewLine,
                exception.Message,
                Environment.NewLine,
                exception.StackTrace);

            Log(severity, messageWithException);
        }

        public void LogFormat(string severity, string format, params object[] args)
        {
            var message = string.Format(format, args);

            Log(severity, message);
        }

        public void Log(string severity, string message)
        {
            var logEntry = new LogEntry
            {
                Severity = this.Severity(severity),
                Message = message, 
                TimeStamp = DateTime.UtcNow, 
            };

            logEntry.ExtendedProperties.Add("Authenticated User", ContextInfoProvider.GetUserName());
            logEntry.ExtendedProperties.Add("Client Machine Name", ContextInfoProvider.GetClientMachineName());

            this.logWriter.Write(logEntry);
        }

        private TraceEventType Severity(string value)
        {
            switch (value)
            {
                case "Debug":
                    return TraceEventType.Verbose;

                case "Info":
                    return TraceEventType.Information;

                case "Warn":
                    return TraceEventType.Warning;

                case "Error":
                    return TraceEventType.Error;

                case "Fatal":
                    return TraceEventType.Error;

                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}