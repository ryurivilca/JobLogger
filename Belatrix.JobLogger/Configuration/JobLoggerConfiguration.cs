using Belatrix.JobLogger.LoggerTypes;

namespace Belatrix.JobLogger.Configuration
{
    public class JobLoggerConfiguration
    {
        public IJobLogger JobLogger { get; set; }
        public bool Enabled { get; set; }

        public JobLoggerConfiguration(IJobLogger logger, bool enabled = false)
        {
            JobLogger = logger;
            Enabled = enabled;
        }
    }
}
