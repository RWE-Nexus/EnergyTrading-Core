namespace EnergyTrading.Test.MoqSpecific
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using EnergyTrading.Logging;

    using Moq;

    /// <summary>
    /// Wraps a Mock ILogger and synchronizes thread access to prevent race conditions 
    /// and concurrent changes to internal collections of the Mock
    /// </summary>
    public class SynchronizedMockLogger : ILogger
    {
        private readonly Mock<ILogger> wrappedMock;
        private readonly object lockObject = new object();
        private readonly List<MockCounter<ILogger>> currentCounters = new List<MockCounter<ILogger>>();

        public SynchronizedMockLogger() : this(new Mock<ILogger>())
        {
        }

        public SynchronizedMockLogger(Mock<ILogger> mockLogger)
        {
            if (mockLogger == null)
            {
                throw new ArgumentNullException("mockLogger");
            }
            this.wrappedMock = mockLogger;
        }

        public void Debug(string message)
        {
            lock(this.lockObject)
            {
                this.wrappedMock.Object.Debug(message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Debug(message, exception);
            }
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.DebugFormat(format, parameters);
            }
        }

        public void Info(string message)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Info(message);
            }
        }

        public void Info(string message, Exception exception)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Info(message, exception);
            }
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.InfoFormat(format, parameters);
            }
        }

        public void Warn(string message)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Warn(message);
            }
        }

        public void Warn(string message, Exception exception)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Warn(message, exception);
            }
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.WarnFormat(format, parameters);
            }
        }

        public void Error(string message)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Error(message, exception);
            }
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.ErrorFormat(format, parameters);
            }
        }

        public void Fatal(string message)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.Fatal(message, exception);
            }
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Object.FatalFormat(format, parameters);
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.wrappedMock.Object.IsDebugEnabled;
                }
            }
            set
            {
                lock (this.lockObject)
                {
                    this.wrappedMock.SetupGet(x => x.IsDebugEnabled).Returns(value);
                }
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.wrappedMock.Object.IsInfoEnabled;
                }
            }
            set
            {
                lock (this.lockObject)
                {
                    this.wrappedMock.SetupGet(x => x.IsInfoEnabled).Returns(value);
                }
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.wrappedMock.Object.IsWarnEnabled;
                }
            }
            set
            {
                lock (this.lockObject)
                {
                    this.wrappedMock.SetupGet(x => x.IsWarnEnabled).Returns(value);
                }
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.wrappedMock.Object.IsErrorEnabled;
                }
            }
            set
            {
                lock (this.lockObject)
                {
                    this.wrappedMock.SetupGet(x => x.IsErrorEnabled).Returns(value);
                }
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.wrappedMock.Object.IsFatalEnabled;
                }
            }
            set
            {
                lock (this.lockObject)
                {
                    this.wrappedMock.SetupGet(x => x.IsFatalEnabled).Returns(value);
                }
            }
        }

        public MockCounter<ILogger> StartCounting(Expression<Action<ILogger>> setupAction)
        {
            lock (this.lockObject)
            {
                var counter = new MockCounter<ILogger>(this.wrappedMock, setupAction);
                this.currentCounters.Add(counter);
                return counter;
            }
        }

        public void StopCounting(MockCounter<ILogger> logCounter)
        {
            lock (this.lockObject)
            {
                logCounter.Dispose();
                this.currentCounters.Remove(logCounter);
            }
        }

        public void Verify(Expression<Action<ILogger>> action)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Verify(action);
            }
        }

        public void Verify(Expression<Action<ILogger>> action, Times times)
        {
            lock(this.lockObject)
            {
                this.wrappedMock.Verify(action, times);
            }
        }

        public void Setup(Expression<Action<ILogger>> action)
        {
            lock (this.lockObject)
            {
                this.wrappedMock.Setup(action);
            }
        }
    }
}