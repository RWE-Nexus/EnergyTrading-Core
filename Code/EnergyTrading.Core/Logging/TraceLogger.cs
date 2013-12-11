namespace EnergyTrading.Logging
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A logger that uses a <see cref="TraceSwitch" /> to decide logging.
    /// </summary>
    public class TraceLogger : ILogger
    {
        private readonly TraceSwitch traceSwitch;
        private readonly Action<string> write;

        public string Category { get; private set; }

        public TraceLogger(TraceSwitch traceSwitch, string category, Action<string, string> write)
        {
            this.traceSwitch = traceSwitch;
            this.Category = category;
            this.write = m => write(m, category); // partial function application, not currying. Apparently.
        }

        /// <inheritdoc />
        /// <remarks>Maps to <see cref="TraceSwitch.TraceVerbose"/></remarks>
        public bool IsDebugEnabled 
        {
            get { return this.traceSwitch.TraceVerbose; }
        }

        /// <inheritdoc />
        /// <remarks>Maps to <see cref="TraceSwitch.TraceInfo"/></remarks>/// 
        public bool IsInfoEnabled
        {
            get { return this.traceSwitch.TraceInfo; }
        }

        /// <inheritdoc />
        /// <remarks>Maps to <see cref="TraceSwitch.TraceWarning"/></remarks>
        public bool IsWarnEnabled
        {
            get { return this.traceSwitch.TraceWarning; }
        }

        /// <inheritdoc />
        /// <remarks>Maps to <see cref="TraceSwitch.TraceError"/></remarks>
        public bool IsErrorEnabled
        {
            get { return this.traceSwitch.TraceError; }
        }

        /// <inheritdoc />
        /// <remarks>Maps to <see cref="TraceSwitch.TraceError"/></remarks>
        public bool IsFatalEnabled
        {
            get { return this.traceSwitch.TraceError; }
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            this.write(message);
        }

        /// <inheritdoc />
        public void Debug(string message, Exception exception)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            this.write(message);
            this.write(exception.ToString());
        }

        /// <inheritdoc />
        public void DebugFormat(string format, params object[] args)
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            this.write(string.Format(format, args));
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            this.write(message);
        }

        /// <inheritdoc />
        public void Info(string message, Exception exception)
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            this.write(message);
            this.write(exception.ToString());
        }

        /// <inheritdoc />
        public void InfoFormat(string format, params object[] args)
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            this.write(string.Format(format, args));
        }

        /// <inheritdoc />
        public void Warn(string message)
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            this.write(message);
        }

        /// <inheritdoc />
        public void Warn(string message, Exception exception)
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            this.write(message);
            this.write(exception.ToString());
        }

        /// <inheritdoc />
        public void WarnFormat(string format, params object[] args)
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            this.write(string.Format(format, args));
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            this.write(message);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception)
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            this.write(message);
            this.write(exception.ToString());
        }

        /// <inheritdoc />
        public void ErrorFormat(string format, params object[] args)
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            this.write(string.Format(format, args));
        }

        /// <inheritdoc />
        public void Fatal(string message)
        {
            if (!IsFatalEnabled)
            {
                return;
            }

            this.write(message);
        }

        /// <inheritdoc />
        public void Fatal(string message, Exception exception)
        {
            if (!IsFatalEnabled)
            {
                return;
            }

            this.write(message);
            this.write(exception.ToString());
        }

        /// <inheritdoc />
        public void FatalFormat(string format, params object[] args)
        {
            if (!IsFatalEnabled)
            {
                return;
            }

            this.write(string.Format(format, args));
        }
    }
}