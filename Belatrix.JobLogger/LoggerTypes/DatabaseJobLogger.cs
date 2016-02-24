using System;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Exceptions;
using Belatrix.JobLogger.Helpers;

namespace Belatrix.JobLogger.LoggerTypes
{
    public interface IDatabaseJobLogger : IJobLogger
    {
    }

    public class DatabaseJobLogger : IDatabaseJobLogger
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly IConfigurationHelper _configurationHelper;

        public DatabaseJobLogger(IDatabaseHelper databaseHelper, IConfigurationHelper configurationHelper)
        {
            _databaseHelper = databaseHelper;
            _configurationHelper = configurationHelper;
        }

        private string ConnectionString
        {
            get
            {
                var config = _configurationHelper.GetLoggerConfigurationSection();

                if (config != null && config.LogTargetConfiguration != null &&
                    string.IsNullOrWhiteSpace(config.LogTargetConfiguration.DatabaseConnectionString))
                {
                    throw new JobLoggerConfigurationException("Could not find databaseConnectionString setting in configuration file.");
                }

                return config.LogTargetConfiguration.DatabaseConnectionString;
            }
        }

        public void LogMessage(string message, SeverityLevel severityLevel)
        {
            _databaseHelper.ConnectionString = ConnectionString;
            _databaseHelper.Insert(message, (int) severityLevel);
        }
    }
}