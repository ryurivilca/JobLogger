using System;
using System.IO;
using Belatrix.JobLogger.Configuration;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Exceptions;
using Belatrix.JobLogger.Helpers;
using Belatrix.JobLogger.LoggerTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Belatrix.JobLogger.Tests.LoggerTypes
{
    [TestClass()]
    public class FileJobLoggerTests
    {
        private FileJobLogger _target;
        private Mock<IFileManager> _fileManager;
        private Mock<IDateTimeHelper> _dateTimeHelper;
        private Mock<IConfigurationHelper> _configurationHelper;

        [TestInitialize]
        public void Initialize()
        {
            _fileManager = new Mock<IFileManager>();
            _dateTimeHelper = new Mock<IDateTimeHelper>();
            _configurationHelper = new Mock<IConfigurationHelper>();
            _target = new FileJobLogger(_fileManager.Object, _dateTimeHelper.Object, _configurationHelper.Object);
        }

        [TestMethod()]
        public void LogMessage_MessageSeverityLevel_CallsAppendTextToFileMethod()
        {
            // Arrange
            const string message = "test_log";
            const SeverityLevel severityLevel = SeverityLevel.Message;
            const string logDirectoryPath = "directory path";
            var dateTimeNow = new DateTime(2016, 01, 02, 10, 11, 12);
            const string filePath = @"directory path\LogFile02-01-2016.txt";
            const string logText = @"10:11:12 - Message - test_log";

            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration {LogDirectoryPath = logDirectoryPath}
            };

            _dateTimeHelper.Setup(x => x.DateTimeNow).Returns(dateTimeNow);
            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);

            _fileManager.Setup(x => x.DirectoryExist(logDirectoryPath)).Returns(true);
            _fileManager.Setup(x => x.AppendTextToFile(filePath, logText)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _fileManager.Verify();
        }

        [TestMethod()]
        public void LogMessage_WarningSeverityLevel_CallsAppendTextToFileMethod()
        {
            // Arrange
            const string message = "test_log";
            const SeverityLevel severityLevel = SeverityLevel.Warning;
            const string logDirectoryPath = "directory path";
            var dateTimeNow = new DateTime(2016, 01, 02, 10, 11, 12);
            const string filePath = @"directory path\LogFile02-01-2016.txt";
            const string logText = @"10:11:12 - Warning - test_log";

            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration { LogDirectoryPath = logDirectoryPath }
            };

            _dateTimeHelper.Setup(x => x.DateTimeNow).Returns(dateTimeNow);
            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);

            _fileManager.Setup(x => x.DirectoryExist(logDirectoryPath)).Returns(true);
            _fileManager.Setup(x => x.AppendTextToFile(filePath, logText)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _fileManager.Verify();
        }

        [TestMethod()]
        public void LogMessage_ErrorSeverityLevel_CallsAppendTextToFileMethod()
        {
            // Arrange
            const string message = "test_log";
            const SeverityLevel severityLevel = SeverityLevel.Error;
            const string logDirectoryPath = "directory path";
            var dateTimeNow = new DateTime(2016, 01, 02, 10, 11, 12);
            const string filePath = @"directory path\LogFile02-01-2016.txt";
            const string logText = @"10:11:12 - Error - test_log";

            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration { LogDirectoryPath = logDirectoryPath }
            };

            _dateTimeHelper.Setup(x => x.DateTimeNow).Returns(dateTimeNow);
            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);

            _fileManager.Setup(x => x.DirectoryExist(logDirectoryPath)).Returns(true);
            _fileManager.Setup(x => x.AppendTextToFile(filePath, logText)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _fileManager.Verify();
        }

        [TestMethod()]
        public void LogMessage_LogDirectoryPathNotFound_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            const string message = "DB message";
            var severityLevel = SeverityLevel.Message;
            const string logDirectoryPath = "directory path";
            DirectoryNotFoundException actualException = null;

            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration { LogDirectoryPath = logDirectoryPath }
            };

            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);
            _fileManager.Setup(x => x.DirectoryExist(logDirectoryPath)).Returns(false);

            try
            {
                // Act
                _target.LogMessage(message, severityLevel);
            }
            catch (DirectoryNotFoundException ex)
            {
                actualException = ex;
            }

            // Assert
            Assert.IsNotNull(actualException);
            Assert.AreEqual("Log directory path does not exist.", actualException.Message);
        }

        [TestMethod()]
        public void LogMessage_LogDirectoryPathSettingDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            const string message = "DB message";
            var severityLevel = SeverityLevel.Message;
            JobLoggerConfigurationException actualException = null;
            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection();

            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);

            try
            {
                // Act
                _target.LogMessage(message, severityLevel);
            }
            catch (JobLoggerConfigurationException ex)
            {
                actualException = ex;
            }

            // Assert
            Assert.IsNotNull(actualException);
            Assert.AreEqual("Could not find logDirectoryPath setting in configuration file.", actualException.Message);
        }
    }
}
