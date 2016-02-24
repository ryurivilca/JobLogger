using System;
using Belatrix.JobLogger.Enums;
using Belatrix.JobLogger.Helpers;

namespace Belatrix.JobLogger.LoggerTypes
{
    public interface IConsoleJobLogger : IJobLogger
    {
    }

    public class ConsoleJobLogger : IConsoleJobLogger
    {
        private readonly IConsoleWriter _console;
        private readonly IDateTimeHelper _dateTimeHelper;

        public ConsoleJobLogger()
        {
            _console = new ConsoleWriter();
        }

        public ConsoleJobLogger(IConsoleWriter console, IDateTimeHelper dateTimeHelper)
        {
            _console = console;
            _dateTimeHelper = dateTimeHelper;
        }

        public void LogMessage(string message, SeverityLevel severityLevel)
        {
            var consoleColor = GetConsoleColor(severityLevel);
            var messageValue = String.Format("{0} {1}", _dateTimeHelper.DateTimeNow.ToShortTimeString(), message);
            _console.WriteLine(messageValue, consoleColor);
        }

        private static ConsoleColor GetConsoleColor(SeverityLevel severityLevel)
        {
            switch (severityLevel)
            {
                case SeverityLevel.Message:
                    return ConsoleColor.White;
                case SeverityLevel.Warning:
                    return ConsoleColor.Yellow;
                case SeverityLevel.Error:
                    return ConsoleColor.Red;
                default:
                    throw new NotSupportedException("Severity Level is not supported.");
            }
        }
    }
}