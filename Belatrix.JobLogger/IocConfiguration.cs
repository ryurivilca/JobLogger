using System.Diagnostics.CodeAnalysis;
using Belatrix.JobLogger.Configuration;
using Belatrix.JobLogger.Helpers;
using Belatrix.JobLogger.LoggerTypes;
using Ninject;

namespace Belatrix.JobLogger
{
    [ExcludeFromCodeCoverage]
    public class IocConfiguration
    {
        private static StandardKernel _kernel;

        public static void Configure()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IConsoleJobLogger>().To<ConsoleJobLogger>();
            _kernel.Bind<IFileJobLogger>().To<FileJobLogger>();
            _kernel.Bind<IDatabaseJobLogger>().To<DatabaseJobLogger>();
            _kernel.Bind<IJobLoggerManager>().To<JobLoggerManager>();

            _kernel.Bind<IJobLoggerConfigurationSection>().To<JobLoggerConfigurationSection>();
            _kernel.Bind<ILogTargetConfiguration>().To<LogTargetConfiguration>();
            _kernel.Bind<ISeverityLevelConfiguration>().To<SeverityLevelConfiguration>();

            _kernel.Bind<IConsoleWriter>().To<ConsoleWriter>();
            _kernel.Bind<IDateTimeHelper>().To<DateTimeHelper>();
            _kernel.Bind<IFileManager>().To<FileManager>();
            _kernel.Bind<IDatabaseHelper>().To<DatabaseHelper>();
            _kernel.Bind<IConfigurationHelper>().To<ConfigurationHelper>();
        }

        public static IJobLoggerManager GetJobLoggerManager()
        {
            return _kernel.Get<IJobLoggerManager>();
        }
    }
}
