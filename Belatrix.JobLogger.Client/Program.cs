using System;
using Belatrix.JobLogger.Enums;

namespace Belatrix.JobLogger.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IocConfiguration.Configure();

            var jobLogger = IocConfiguration.GetJobLoggerManager();

            while (true)
            {
                Console.WriteLine("Type a message");
                var message = Console.ReadLine();
                Console.WriteLine("Type a message type (1 = Message, 2 = Error, 3=Warning)");

                var typeStr = Console.ReadLine();
                int typeInt;

                if (int.TryParse(typeStr, out typeInt))
                {
                    var type = (SeverityLevel)typeInt;

                    try
                    {
                        jobLogger.LogMessage(message, type);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("System Error: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("System Error: The type must be a number");
                }
                Console.WriteLine();
            }
        }
    }
}
