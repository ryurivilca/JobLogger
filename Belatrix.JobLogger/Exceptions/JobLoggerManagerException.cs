using System;

namespace Belatrix.JobLogger.Exceptions
{
    public class JobLoggerManagerException : Exception
    {
        public JobLoggerManagerException()
        {
        }

        public JobLoggerManagerException(string message)
            : base(message)
        {
        }

        public JobLoggerManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
