using System;

namespace Belatrix.JobLogger.Exceptions
{
    public class JobLoggerConfigurationException : Exception
    {
         public JobLoggerConfigurationException()
        {
        }

        public JobLoggerConfigurationException(string message)
            : base(message)
        {
        }

        public JobLoggerConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
