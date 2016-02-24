using System;
using System.Diagnostics.CodeAnalysis;

namespace Belatrix.JobLogger.Helpers
{
    public interface IConsoleWriter
    {
        void WriteLine(string value, ConsoleColor color);
    }

    [ExcludeFromCodeCoverage]
    public class ConsoleWriter : IConsoleWriter
    {
        public void WriteLine(string value, ConsoleColor color)
        {
            Console.WriteLine(value);
            Console.ForegroundColor = color;
        }
    }
}
