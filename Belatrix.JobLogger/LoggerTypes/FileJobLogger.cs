using System;
using System.IO;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Exceptions;
using Belatrix.JobLogger.Helpers;

namespace Belatrix.JobLogger.LoggerTypes
{
    public interface IFileJobLogger : IJobLogger
    {
    }

    public class FileJobLogger : IFileJobLogger
    {
        private readonly IFileManager _fileManager;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IConfigurationHelper _configurationHelper;

        public FileJobLogger(IFileManager fileManager, IDateTimeHelper dateTimeHelper,
            IConfigurationHelper configurationHelper)
        {
            _fileManager = fileManager;
            _dateTimeHelper = dateTimeHelper;
            _configurationHelper = configurationHelper;
        }

        private string LogDirectoryPath
        {
            get
            {
                var config = _configurationHelper.GetLoggerConfigurationSection();

                if (config != null && config.LogTargetConfiguration != null &&
                    string.IsNullOrWhiteSpace(config.LogTargetConfiguration.LogDirectoryPath))
                {
                    throw new JobLoggerConfigurationException("Could not find logDirectoryPath setting in configuration file.");
                }

                return config.LogTargetConfiguration.LogDirectoryPath;
            }
        }

        public void LogMessage(string message, SeverityLevel severityLevel)
        {
            var fileName = String.Format("LogFile{0}.txt", _dateTimeHelper.DateTimeNow.ToString("dd-MM-yyyy"));

            if (!_fileManager.DirectoryExist(LogDirectoryPath))
                throw new DirectoryNotFoundException("Log directory path does not exist.");

            var filePath = Path.Combine(LogDirectoryPath, fileName);
            var logText = String.Format("{0} - {1} - {2}", _dateTimeHelper.DateTimeNow.ToString("H:mm:ss"), severityLevel,
                message);

            _fileManager.AppendTextToFile(filePath, logText);
        }
    }
}