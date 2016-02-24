using System.Configuration;

namespace Belatrix.JobLogger.Configuration
{
    public interface ISeverityLevelConfiguration
    {
        bool LogMessage { get; set; }
        bool LogWarning { get; set; }
        bool LogError { get; set; }
    }

    public class SeverityLevelConfiguration : ConfigurationElement, ISeverityLevelConfiguration
    {
        [ConfigurationProperty("logMessage", DefaultValue = false, IsRequired = true)]
        public bool LogMessage
        {
            get { return (bool)this["logMessage"]; }
            set { this["logMessage"] = value; }
        }

        [ConfigurationProperty("logWarning", DefaultValue = false, IsRequired = true)]
        public bool LogWarning
        {
            get { return (bool)this["logWarning"]; }
            set { this["logWarning"] = value; }
        }

        [ConfigurationProperty("logError", DefaultValue = false, IsRequired = true)]
        public bool LogError
        {
            get { return (bool)this["logError"]; }
            set { this["logError"] = value; }
        }
    }
}
