namespace EnergyTrading.Logging
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A logger factory using the EnergyTrading.Logging.<see cref="TraceSwitch" /> to determine logging.
    /// </summary>
    public class TraceLoggerFactory : ILoggerFactory
    {
        private readonly TraceSwitch traceSwitch;

        public TraceLoggerFactory() : this(new TraceSwitch("EnergyTrading.Logging", "Trace switch for EnergyTrading.Logging"))
        {
        }

        public TraceLoggerFactory(TraceSwitch traceSwitch)
        {
            this.traceSwitch = traceSwitch;
        }

        /// <inheritdoc />
        public ILogger GetLogger(string category)
        {
            return new TraceLogger(this.traceSwitch, category, this.Write);
        }

        /// <inheritdoc />
        public ILogger GetLogger<T>()
        {
            return this.GetLogger(typeof(T));
        }

        /// <inheritdoc />
        public ILogger GetLogger(Type type)
        {
            return this.GetLogger(type.FullName);
        }

        /// <inheritdoc />
        public void Initialize()
        {
        }

        /// <inheritdoc />
        public void Shutdown()
        {            
        }

        /// <inheritdoc />
        private void Write(string message, string category)
        {
            Trace.WriteLine(message, category);
        }
    }
}