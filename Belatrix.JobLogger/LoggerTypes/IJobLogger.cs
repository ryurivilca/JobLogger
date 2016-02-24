using Belatrix.JobLogger.Enums;

namespace Belatrix.JobLogger.LoggerTypes
{
    public interface IJobLogger
    {
        void LogMessage(string message, SeverityLevel severityLevel);
    }
}
