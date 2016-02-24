using System.Configuration;

namespace Belatrix.JobLogger.Configuration
{
    public interface IJobLoggerConfigurationSection
    {
        SeverityLevelConfiguration SeverityLevelConfiguration { get; set; }
        LogTargetConfiguration LogTargetConfiguration { get; set; }
    }

    public class JobLoggerConfigurationSection : ConfigurationSection, IJobLoggerConfigurationSection
    {
        [ConfigurationProperty("severityLevelConfiguration")]
        public SeverityLevelConfiguration SeverityLevelConfiguration
        {
            get { return (SeverityLevelConfiguration)this["severityLevelConfiguration"]; }
            set { this["severityLevelConfiguration"] = value; }
        }

        [ConfigurationProperty("logTargetConfiguration")]
        public LogTargetConfiguration LogTargetConfiguration
        {
            get { return (LogTargetConfiguration)this["logTargetConfiguration"]; }
            set { this["logTargetConfiguration"] = value; }
        }
    }
}
