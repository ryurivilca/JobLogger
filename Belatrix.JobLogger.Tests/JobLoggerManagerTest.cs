using System;
using Belatrix.JobLogger.Configuration;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Exceptions;
using Belatrix.JobLogger.Helpers;
using Belatrix.JobLogger.LoggerTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Belatrix.JobLogger.Tests
{
    [TestClass]
    public class JobLoggerManagerTest
    {
        private Mock<IConsoleJobLogger> _consoleJobLogger;
        private Mock<IFileJobLogger> _fileJobLogger;
        private Mock<IDatabaseJobLogger> _databaseJobLogger;
        private Mock<IConfigurationHelper> _configurationHelper;
        
        private string _message = "any message";
        private SeverityLevel _severityLevel = SeverityLevel.Message;
        private JobLoggerConfigurationSection jobLoggerConfigurationSection;

        [TestInitialize]
        public void TestInitialize()
        {
            _consoleJobLogger = new Mock<IConsoleJobLogger>();
            _fileJobLogger = new Mock<IFileJobLogger>();
            _databaseJobLogger = new Mock<IDatabaseJobLogger>();
            _configurationHelper = new Mock<IConfigurationHelper>();
        }

        [TestMethod]
        public void LogMessage_LogToConsoleTargetEnabled_CallsConsoleLoggerOnly()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = false, LogToDatabase = false },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = true, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
               _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel));
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
        }

        [TestMethod]
        public void LogMessage_LogToFileTargetEnabled_CallsFileLoggerOnly()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = false, LogToFile = true, LogToDatabase = false },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = true, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
               _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel));
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
        }

        [TestMethod]
        public void LogMessage_LogToDatabaseTargetEnabled_CallsDatabaseLoggerOnly()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = false, LogToFile = false, LogToDatabase = true },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = true, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
               _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel));
        }

        [TestMethod]
        public void LogMessage_AllLogTargetsEnabled_CallsAllJobLoggers()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = true, LogToDatabase = true },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = true, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
               _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel));
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel));
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel));
        }

        [TestMethod]
        public void LogMessage_NoneLogTargetEnabled_NonCallsToAnyJobLogger()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = false, LogToFile = false, LogToDatabase = false },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = true, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
               _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(JobLoggerManagerException))]
        public void LogMessage_ExceptionInLoggerClass_ThrowsJobLoggerManagerException()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = false, LogToDatabase = false },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = true, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);


            _consoleJobLogger.Setup(m => m.LogMessage(_message, _severityLevel)).Throws<JobLoggerManagerException>();

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
                _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
        }

        [TestMethod]
        public void LogMessage_LogMessageEnabled_AllowLogMessage()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = true, LogToDatabase = true },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = true, LogError = false, LogWarning = false }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
              _configurationHelper.Object);

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
        }

        [TestMethod]
        public void LogMessage_LogErrorEnabled_AllowErrorMessage()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = true, LogToDatabase = true },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = false, LogError = true, LogWarning = false }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
             _configurationHelper.Object);

            _severityLevel = SeverityLevel.Error;

            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
        }

        [TestMethod]
        public void LogMessage_LogWarningEnabled_AllowWarningMessage()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = true, LogToDatabase = true },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = false, LogError = false, LogWarning = true }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
            _configurationHelper.Object);

            _severityLevel = SeverityLevel.Warning;
            
            //Act
            target.LogMessage(_message, _severityLevel);

            //Assert
            _consoleJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
            _fileJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
            _databaseJobLogger.Verify(m => m.LogMessage(_message, _severityLevel), Times.Once);
        }

        [TestMethod]
        public void LogMessage_NoSeveryLevelEnabled_ThrowsJobLoggerManagerException()
        {
            //Arrange
            jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration =
                    new LogTargetConfiguration { LogToConsole = true, LogToFile = true, LogToDatabase = true },
                SeverityLevelConfiguration =
                    new SeverityLevelConfiguration { LogMessage = false, LogError = false, LogWarning = false }
            };
            _configurationHelper.Setup(c => c.GetLoggerConfigurationSection())
                .Returns(() => jobLoggerConfigurationSection);

            var target = new JobLoggerManager(_consoleJobLogger.Object, _fileJobLogger.Object, _databaseJobLogger.Object,
               _configurationHelper.Object);
            JobLoggerManagerException actualException = null;

            try
            {
                //Act
                target.LogMessage(_message, _severityLevel);
            }
            catch (JobLoggerManagerException ex)
            {
                actualException = ex;
            }

            // Assert
            Assert.IsNotNull(actualException);
            Assert.AreEqual("Error or Warning or Message must be specified", actualException.Message);
        }
    }
}
