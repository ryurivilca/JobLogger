using System;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Helpers;
using Belatrix.JobLogger.LoggerTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Belatrix.JobLogger.Tests.LoggerTypes
{
    [TestClass()]
    public class ConsoleJobLoggerTests
    {
        private ConsoleJobLogger _target;
        private Mock<IConsoleWriter> _consoleWriter;
        private Mock<IDateTimeHelper> _dateTimeHelper;

        [TestInitialize]
        public void Initialize()
        {
            _consoleWriter = new Mock<IConsoleWriter>();
            _dateTimeHelper = new Mock<IDateTimeHelper>();
            _target = new ConsoleJobLogger(_consoleWriter.Object, _dateTimeHelper.Object);
        }

        [TestMethod()]
        public void LogMessage_MessageSeverityLevel_LogsMessageInWhiteColor()
        {
            // Arrange
            const string message = "white message";
            var dateTimeNow = new DateTime(2016, 01, 02, 10, 11, 12);
            var severityLevel = SeverityLevel.Message;
            var messageValue = String.Format("{0} {1}", dateTimeNow.ToShortTimeString(), message);

            _dateTimeHelper.Setup(x => x.DateTimeNow).Returns(dateTimeNow);
            _consoleWriter.Setup(x => x.WriteLine(messageValue, ConsoleColor.White)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _consoleWriter.Verify();
        }

        [TestMethod()]
        public void LogMessage_WarningSeverityLevel_LogsMessageInYellowColor()
        {
            // Arrange
            const string message = "yellow message";
            var dateTimeNow = new DateTime(2016, 01, 02, 10, 11, 12);
            var severityLevel = SeverityLevel.Warning;
            var messageValue = String.Format("{0} {1}", dateTimeNow.ToShortTimeString(), message);

            _dateTimeHelper.Setup(x => x.DateTimeNow).Returns(dateTimeNow);
            _consoleWriter.Setup(x => x.WriteLine(messageValue, ConsoleColor.Yellow)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _consoleWriter.Verify();
        }

        [TestMethod()]
        public void LogMessage_ErrorSeverityLevel_LogsMessageInRedColor()
        {
            // Arrange
            const string message = "red message";
            var dateTimeNow = new DateTime(2016, 01, 02, 10, 11, 12);
            var severityLevel = SeverityLevel.Error;
            var messageValue = String.Format("{0} {1}", dateTimeNow.ToShortTimeString(), message);

            _dateTimeHelper.Setup(x => x.DateTimeNow).Returns(dateTimeNow);
            _consoleWriter.Setup(x => x.WriteLine(messageValue, ConsoleColor.Red)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _consoleWriter.Verify();
        }
        
        [TestMethod()]
        public void LogMessage_NoneSeverityLevel_ThrowsNotSupportedException()
        {
            // Arrange
            const string message = "heyo";
            var severityLevel = SeverityLevel.None;
            NotSupportedException actualException = null;

            try
            {
                // Act
                _target.LogMessage(message, severityLevel);
            }
            catch (NotSupportedException ex)
            {
                actualException = ex;
            }

            // Assert
            Assert.IsNotNull(actualException);
            Assert.AreEqual("Severity Level is not supported.", actualException.Message);
        }

    }
}
