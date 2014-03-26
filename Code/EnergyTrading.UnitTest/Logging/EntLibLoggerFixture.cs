namespace EnergyTrading.UnitTest.Logging
{
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using NUnit.Framework;

    using Moq;

    using EnergyTrading.Logging.EnterpriseLibrary;

    using SharpTestsEx;

    [TestFixture]
    public class EntLibLoggerFixture
    {
        [Test]
        public void ShouldAlwaysIncludeContextUserNameInLogEntry()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test error message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Error(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.ExtendedProperties.ContainsKey("Authenticated User").Should().Be.True();
        }

        [Test]
        public void ShouldAlwaysIncludeClientMachineNameInLogEntry()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test error message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Error(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.ExtendedProperties.ContainsKey("Client Machine Name").Should().Be.True();
        }

        [Test]
        public void ShouldAlwaysAddMessageToLogEntry()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test error message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Error(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.Message.Should().Not.Be.NullOrEmpty();
            logEntry.Message.Should().Be(TestMessage);
        }

        [Test]
        public void ShouldLogDebugMessagesWithSeverityAsVerbose()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test debug message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Debug(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.Message.Should().Not.Be.NullOrEmpty();
            logEntry.Message.Should().Be(TestMessage);
            logEntry.Severity.Should().Be(TraceEventType.Verbose);
        }

        [Test]
        public void ShouldLogInformationMessagesWithSeverityAsInformation()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test info message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Info(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.Message.Should().Not.Be.NullOrEmpty();
            logEntry.Message.Should().Be(TestMessage);
            logEntry.Severity.Should().Be(TraceEventType.Information);
        }

        [Test]
        public void ShouldLogExceptionMessagesWithSeverityAsError()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test error message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Error(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.Message.Should().Not.Be.NullOrEmpty();
            logEntry.Message.Should().Be(TestMessage);
            logEntry.Severity.Should().Be(TraceEventType.Error);
        }

        [Test]
        public void ShouldLogWarningMessagesWithSeverityAsWarning()
        {
            // Given
            var mockLogWritter = new Mock<LogWriter>();
            var logger = new EntLibLogger(mockLogWritter.Object);
            const string TestMessage = "This is test warning message";
            LogEntry logEntry = null;

            // When
            mockLogWritter.Setup(x => x.Write(It.IsAny<LogEntry>())).Callback((LogEntry le) => { logEntry = le; });
            logger.Warn(TestMessage);

            // Then
            logEntry.Should().Not.Be.Null();
            logEntry.Message.Should().Not.Be.NullOrEmpty();
            logEntry.Message.Should().Be(TestMessage);
            logEntry.Severity.Should().Be(TraceEventType.Warning);
        }
    }
}