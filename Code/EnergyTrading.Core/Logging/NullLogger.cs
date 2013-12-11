namespace EnergyTrading.Logging
{
    using System;

    /// <summary>
    /// Null implementation of <see cref="ILogger" />. All actions do nothing.
    /// </summary>
    /// <remarks>This allows a stub behaviour to be registered, a version of the null object pattern.</remarks>
    public class NullLogger : ILogger
    {
        private readonly bool shouldLog;

        public NullLogger() : this(false)
        {            
        }

        public NullLogger(bool shouldLog)
        {
            this.shouldLog = shouldLog;
        }

        /// <inheritdoc />
        public bool IsDebugEnabled
        {
            get { return this.shouldLog; }
        }

        /// <inheritdoc />
        public bool IsInfoEnabled
        {
            get { return this.shouldLog; }
        }

        /// <inheritdoc />
        public bool IsWarnEnabled
        {
            get { return this.shouldLog; }
        }

        /// <inheritdoc />
        public bool IsErrorEnabled
        {
            get { return this.shouldLog; }
        }

        /// <inheritdoc />
        public bool IsFatalEnabled
        {
            get { return this.shouldLog; }
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
        }

        /// <inheritdoc />
        public void Debug(string message, Exception exception)
        {
        }

        /// <inheritdoc />
        public void DebugFormat(string format, params object[] args)
        {
        }

        /// <inheritdoc />
        public void Info(string message)
        {
        }

        /// <inheritdoc />
        public void Info(string message, Exception exception)
        {
        }

        /// <inheritdoc />
        public void InfoFormat(string format, params object[] args)
        {
        }

        /// <inheritdoc />
        public void Warn(string message)
        {
        }

        /// <inheritdoc />
        public void Warn(string message, Exception exception)
        {
        }

        /// <inheritdoc />
        public void WarnFormat(string format, params object[] args)
        {
        }

        /// <inheritdoc />
        public void Error(string message)
        {
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception)
        {
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, params object[] args)
        {
        }

        /// <inheritdoc />
        public void Fatal(string message)
        {
        }

        /// <inheritdoc />
        public void Fatal(string message, Exception exception)
        {
        }

        /// <inheritdoc />
        public void FatalFormat(string format, params object[] args)
        {
        }
    }
}