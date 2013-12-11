namespace EnergyTrading.Test.MoqSpecific
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Moq;

    using EnergyTrading.Logging;

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
            wrappedMock = mockLogger;
        }

        public void Debug(string message)
        {
            lock(lockObject)
            {
                wrappedMock.Object.Debug(message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Debug(message, exception);
            }
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            lock (lockObject)
            {
                wrappedMock.Object.DebugFormat(format, parameters);
            }
        }

        public void Info(string message)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Info(message);
            }
        }

        public void Info(string message, Exception exception)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Info(message, exception);
            }
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            lock (lockObject)
            {
                wrappedMock.Object.InfoFormat(format, parameters);
            }
        }

        public void Warn(string message)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Warn(message);
            }
        }

        public void Warn(string message, Exception exception)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Warn(message, exception);
            }
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            lock (lockObject)
            {
                wrappedMock.Object.WarnFormat(format, parameters);
            }
        }

        public void Error(string message)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Error(message, exception);
            }
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            lock (lockObject)
            {
                wrappedMock.Object.ErrorFormat(format, parameters);
            }
        }

        public void Fatal(string message)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            lock (lockObject)
            {
                wrappedMock.Object.Fatal(message, exception);
            }
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            lock (lockObject)
            {
                wrappedMock.Object.FatalFormat(format, parameters);
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                lock (lockObject)
                {
                    return wrappedMock.Object.IsDebugEnabled;
                }
            }
            set
            {
                lock (lockObject)
                {
                    wrappedMock.SetupGet(x => x.IsDebugEnabled).Returns(value);
                }
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                lock (lockObject)
                {
                    return wrappedMock.Object.IsInfoEnabled;
                }
            }
            set
            {
                lock (lockObject)
                {
                    wrappedMock.SetupGet(x => x.IsInfoEnabled).Returns(value);
                }
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                lock (lockObject)
                {
                    return wrappedMock.Object.IsWarnEnabled;
                }
            }
            set
            {
                lock (lockObject)
                {
                    wrappedMock.SetupGet(x => x.IsWarnEnabled).Returns(value);
                }
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                lock (lockObject)
                {
                    return wrappedMock.Object.IsErrorEnabled;
                }
            }
            set
            {
                lock (lockObject)
                {
                    wrappedMock.SetupGet(x => x.IsErrorEnabled).Returns(value);
                }
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                lock (lockObject)
                {
                    return wrappedMock.Object.IsFatalEnabled;
                }
            }
            set
            {
                lock (lockObject)
                {
                    wrappedMock.SetupGet(x => x.IsFatalEnabled).Returns(value);
                }
            }
        }

        public MockCounter<ILogger> StartCounting(Expression<Action<ILogger>> setupAction)
        {
            lock (lockObject)
            {
                var counter = new MockCounter<ILogger>(wrappedMock, setupAction);
                currentCounters.Add(counter);
                return counter;
            }
        }

        public void StopCounting(MockCounter<ILogger> logCounter)
        {
            lock (lockObject)
            {
                logCounter.Dispose();
                currentCounters.Remove(logCounter);
            }
        }

        public void Verify(Expression<Action<ILogger>> action)
        {
            lock (lockObject)
            {
                wrappedMock.Verify(action);
            }
        }

        public void Verify(Expression<Action<ILogger>> action, Times times)
        {
            lock(lockObject)
            {
                wrappedMock.Verify(action, times);
            }
        }

        public void Setup(Expression<Action<ILogger>> action)
        {
            lock (lockObject)
            {
                wrappedMock.Setup(action);
            }
        }
    }
}