using System;
using System.Collections.Generic;
using System.Linq;
using Belatrix.JobLogger.Configuration;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Exceptions;
using Belatrix.JobLogger.Helpers;
using Belatrix.JobLogger.LoggerTypes;

namespace Belatrix.JobLogger
{
    public interface IJobLoggerManager
    {
        void LogMessage(string message, SeverityLevel type);
    }

    public class JobLoggerManager : IJobLoggerManager
    {
        private readonly IConsoleJobLogger _consoleJobLogger;
        private readonly IFileJobLogger _fileJobLogger;
        private readonly IDatabaseJobLogger _databaseJobLogger;

        private readonly IConfigurationHelper _configurationHelper;
        private IList<JobLoggerConfiguration> _jobLoggerConfigurations;
        private IList<SeverityLevel> _severityLevels;

        public JobLoggerManager(IConsoleJobLogger consoleJobLogger, IFileJobLogger fileJobLogger,
            IDatabaseJobLogger databaseJobLogger, IConfigurationHelper configurationHelper)
        {
            _consoleJobLogger = consoleJobLogger;
            _fileJobLogger = fileJobLogger;
            _databaseJobLogger = databaseJobLogger;
            _configurationHelper = configurationHelper;
            
            Configure();
        }

        private void Configure()
        {
            var config = _configurationHelper.GetLoggerConfigurationSection();

            LoadJobLoggers(config.LogTargetConfiguration);
            AddSeverityLevels(config.SeverityLevelConfiguration);
        }

        private void LoadJobLoggers(ILogTargetConfiguration targetConfiguration)
        {
            _jobLoggerConfigurations = new List<JobLoggerConfiguration>
            {
                new JobLoggerConfiguration(_consoleJobLogger, targetConfiguration.LogToConsole),
                new JobLoggerConfiguration(_fileJobLogger, targetConfiguration.LogToFile),
                new JobLoggerConfiguration(_databaseJobLogger, targetConfiguration.LogToDatabase)
            };
        }

        private void AddSeverityLevels(ISeverityLevelConfiguration severityLevel)
        {
            _severityLevels = new List<SeverityLevel>();
            if (severityLevel.LogMessage) _severityLevels.Add(SeverityLevel.Message);
            if (severityLevel.LogWarning) _severityLevels.Add(SeverityLevel.Warning);
            if (severityLevel.LogError) _severityLevels.Add(SeverityLevel.Error);
        }

        public void LogMessage(string message, SeverityLevel type)
        {
            if (string.IsNullOrEmpty(message)) return;

            ValidateMessageType();

            if (!_severityLevels.Contains(type)) return;

            ValidateLogTarget();

            LogMessageToTargets(message, type);
        }

        private void LogMessageToTargets(string message, SeverityLevel type)
        {
            try
            {
                foreach (var loggerConfiguration in _jobLoggerConfigurations.Where(x => x.Enabled))
                {

                    loggerConfiguration.JobLogger.LogMessage(message.Trim(), type);
                }
            }
            catch (Exception ex)
            {
                throw new JobLoggerManagerException(ex.Message, ex);
            }
        }

        private void ValidateLogTarget()
        {
            if (_jobLoggerConfigurations.Count == 0)
            {
                throw new Exception("Invalid configuration");
            }
        }

        private void ValidateMessageType()
        {
            if ((_severityLevels.Count == 0))
            {
                throw new JobLoggerManagerException("Error or Warning or Message must be specified");
            }
        }
    }
}