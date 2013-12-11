namespace EnergyTrading.Logging
{
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Implementation of <see cref="ILogger"/> that logs into a <see cref="TextWriter"/>.
    /// </summary>
    public class TextLogger : ILogger, IDisposable
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of <see cref="TextLogger"/> that writes to
        /// the console output.
        /// </summary>
        public TextLogger() : this(Console.Out)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TextLogger"/>.
        /// </summary>
        /// <param name="writer">The writer to use for writing log entries.</param>
        public TextLogger(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this.writer = writer;
            this.IsDebugEnabled = true;
            this.IsInfoEnabled = true;
            this.IsWarnEnabled = true;
            this.IsErrorEnabled = true;
            this.IsFatalEnabled = true;
        }

        public bool IsDebugEnabled { get; set; }

        public bool IsInfoEnabled { get; set; }

        public bool IsWarnEnabled { get; set; }

        public bool IsErrorEnabled { get; set; }

        public bool IsFatalEnabled { get; set; }

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

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the associated <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, disposes the associated <see cref="TextWriter"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.writer != null)
            {
                this.writer.Dispose();
            }
        }

        private void Log(string severity, string message, Exception exception)
        {
            this.Log(severity, message);
            this.Log(severity, exception.ToString());
        }

        private void LogFormat(string severity, string format, params object[] args)
        {
            this.Log(severity, string.Format(format, args));
        }

        /// <summary>
        /// Write a new log entry with the specified severity
        /// </summary>
        /// <param name="severity">Severity of the entry.</param>
        /// <param name="message">Message body to log.</param>
        private void Log(string severity, string message)
        {
            var messageToLog = string.Format(
                CultureInfo.InvariantCulture,
                @"{2:u} - {1}: {0}",
                severity, 
                message, 
                DateTime.UtcNow);

            writer.WriteLine(messageToLog);
        }
    }
}