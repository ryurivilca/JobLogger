using System.Configuration;

namespace Belatrix.JobLogger.Configuration
{
    public interface ILogTargetConfiguration
    {
        bool LogToConsole { get; set; }
        bool LogToFile { get; set; }
        bool LogToDatabase { get; set; }
        string DatabaseConnectionString { get; set; }
        string LogDirectoryPath { get; set; }
    }

    public class LogTargetConfiguration : ConfigurationElement, ILogTargetConfiguration
    {
        [ConfigurationProperty("logToConsole", DefaultValue = false, IsRequired = true, IsKey = true)]
        public bool LogToConsole
        {
            get { return (bool)this["logToConsole"]; }
            set { this["logToConsole"] = value; }
        }

        [ConfigurationProperty("logToFile", DefaultValue = false, IsRequired = true, IsKey = true)]
        public bool LogToFile
        {
            get { return (bool)this["logToFile"]; }
            set { this["logToFile"] = value; }
        }

        [ConfigurationProperty("logToDatabase", DefaultValue = false, IsRequired = true, IsKey = true)]
        public bool LogToDatabase
        {
            get { return (bool)this["logToDatabase"]; }
            set { this["logToDatabase"] = value; }
        }

        [ConfigurationProperty("databaseConnectionString", IsRequired = false, IsKey = false)]
        public string DatabaseConnectionString
        {
            get { return (string)this["databaseConnectionString"]; }
            set { this["databaseConnectionString"] = value; }
        }

        [ConfigurationProperty("logDirectoryPath", IsRequired = false, IsKey = false)]
        public string LogDirectoryPath
        {
            get { return (string)this["logDirectoryPath"]; }
            set { this["logDirectoryPath"] = value; }
        }
    }
}
