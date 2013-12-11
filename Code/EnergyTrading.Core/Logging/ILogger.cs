namespace EnergyTrading.Logging
{
    using System;

    /// <summary>
    /// Logs messages of various categories
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets whether the debug level of logging is enabled.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Gets whether the info level of logging is enabled.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Gets whether the warning level of logging is enabled.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Gets whether the error level of logging is enabled.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Gets whether the fatal level of logging is enabled.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Debug(string message);

        /// <summary>
        /// Log a debug message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to record</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="parameters">Parameters to fill in the format</param>
        void DebugFormat(string format, params object[] parameters);

        /// <summary>
        /// Log an info message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Info(string message);

        /// <summary>
        /// Log an info message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to record</param>
        void Info(string message, Exception exception);

        /// <summary>
        /// Log an info message.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="parameters">Parameters to fill in the format</param>
        void InfoFormat(string format, params object[] parameters);

        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Warn(string message);

        /// <summary>
        /// Log a warning with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to record</param>
        void Warn(string message, Exception exception);

        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="parameters">Parameters to fill in the format</param>
        void WarnFormat(string format, params object[] parameters);

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Error(string message);

        /// <summary>
        /// Log an error message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="parameters">Parameters to fill in the format</param>
        void ErrorFormat(string format, params object[] parameters);

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Fatal(string message);

        /// <summary>
        /// Log a fatal message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="parameters">Parameters to fill in the format</param>
        void FatalFormat(string format, params object[] parameters);
    }
}