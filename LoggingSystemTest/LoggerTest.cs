using CoreWinSubLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingSystemTest
{
    class LoggerTest
    {
        public static void TestNullLogger()
        {
            Logger logger = LogManager.GetLogger("");
            TestLogger(logger);
            Console.WriteLine("NullLogger test, nothing should be print out.");
        }

        public static void TestConsoleLogger()
        {
            LogManager.SetImplementation(ConsoleLogManager.Instance);
            Logger logger = LogManager.GetLogger("");
            TestLogger(logger);
            Console.WriteLine("ConsoleLogger test, 5 messages should be print out.");
        }

        private static void TestLogger(Logger logger)
        {
            logger.Debug("This is a test message with {level}.",DateTime.Now, "Debug");
            logger.Info("This is a test message with {level}.", DateTime.Now, "Info");
            logger.Warn("This is a test message with {level}.", DateTime.Now, "Warn");
            logger.Error("This is a test message with {level}.", DateTime.Now, "Error");
            logger.Fatal("This is a test message with {level}.", DateTime.Now, "Fatal");
        }
    }
}
