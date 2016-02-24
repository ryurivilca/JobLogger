using System;
using System.Diagnostics.CodeAnalysis;

namespace Belatrix.JobLogger.Helpers
{
    public interface IDateTimeHelper
    {
        DateTime DateTimeNow { get; }
    }

    [ExcludeFromCodeCoverage]
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime DateTimeNow
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
