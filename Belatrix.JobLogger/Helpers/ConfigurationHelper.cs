using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Belatrix.JobLogger.Configuration;

namespace Belatrix.JobLogger.Helpers
{
    public interface IConfigurationHelper
    {
        IJobLoggerConfigurationSection GetLoggerConfigurationSection();
    }

    [ExcludeFromCodeCoverage]
    public class ConfigurationHelper : IConfigurationHelper
    {
        public IJobLoggerConfigurationSection GetLoggerConfigurationSection()
        {
            return
                (IJobLoggerConfigurationSection) ConfigurationManager.GetSection("jobLoggerConfiguration");
        }
    }
}
