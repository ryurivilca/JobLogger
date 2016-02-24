using System;
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
    public class DatabaseJobLoggerTests
    {
        private DatabaseJobLogger _target;
        private Mock<IDatabaseHelper> _databaseHelper;
        private Mock<IConfigurationHelper> _configurationHelper;

        [TestInitialize]
        public void Initialize()
        {
            _databaseHelper = new Mock<IDatabaseHelper>();
            _configurationHelper = new Mock<IConfigurationHelper>();
            _target = new DatabaseJobLogger(_databaseHelper.Object, _configurationHelper.Object);
        }
        
        [TestMethod()]
        public void LogMessage_MessageSeverityLevel_CallsInsertMethod()
        {
            // Arrange
            const string message = "DB message";
            const SeverityLevel severityLevel = SeverityLevel.Message;
            const string databaseConnectionString = "db connection string";
            
            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration()
                {
                    DatabaseConnectionString = databaseConnectionString
                }
            };

            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);
            _databaseHelper.SetupSet(x => x.ConnectionString = databaseConnectionString).Verifiable();
            _databaseHelper.Setup(x => x.Insert(message, 1)).Verifiable();
            
            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _databaseHelper.Verify();
        }

        [TestMethod()]
        public void LogMessage_ErrorSeverityLevel_CallsInsertMethod()
        {
            // Arrange
            const string message = "DB message";
            const SeverityLevel severityLevel = SeverityLevel.Error;
            const string databaseConnectionString = "db connection string";

            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration()
                {
                    DatabaseConnectionString = databaseConnectionString
                }
            };

            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);
            _databaseHelper.SetupSet(x => x.ConnectionString = databaseConnectionString).Verifiable();
            _databaseHelper.Setup(x => x.Insert(message, 2)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _databaseHelper.Verify();
        }

        [TestMethod()]
        public void LogMessage_WarningSeverityLevel_CallsInsertMethod()
        {
            // Arrange
            const string message = "DB message";
            const SeverityLevel severityLevel = SeverityLevel.Warning;
            const string databaseConnectionString = "db connection string";

            var jobLoggerConfigurationSection = new JobLoggerConfigurationSection
            {
                LogTargetConfiguration = new LogTargetConfiguration()
                {
                    DatabaseConnectionString = databaseConnectionString
                }
            };

            _configurationHelper.Setup(x => x.GetLoggerConfigurationSection()).Returns(jobLoggerConfigurationSection);
            _databaseHelper.SetupSet(x => x.ConnectionString = databaseConnectionString).Verifiable();
            _databaseHelper.Setup(x => x.Insert(message, 3)).Verifiable();

            // Act
            _target.LogMessage(message, severityLevel);

            // Assert
            _databaseHelper.Verify();
        }

        [TestMethod()]
        public void LogMessage_DatabaseConnectionStringDoesNotExist_ThrowsException()
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
            Assert.AreEqual("Could not find databaseConnectionString setting in configuration file.", actualException.Message);
        }
    }
}
