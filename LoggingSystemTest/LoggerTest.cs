using CoreWinSubLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void TestWcfLogger()
        {
            LogManager.SetImplementation(new WcfLoggerManager("127.0.0.1"));
            Logger logger = LogManager.GetLogger("");
            Enumerable.Range(0, 100).AsParallel().ForAll(t =>
            {
                TestLogger(logger);
            });
        }

        public static void TestLogRecordString()
        {
            LogRecord record = LogRecord.Create(LogLevel.Fatal, "message");
            string recordString = record.ToString();
            LogRecord r2 = LogRecord.FromString(recordString);
            Debug.Assert(r2.Level == LogLevel.Fatal);
            Debug.Assert((r2.DateTime - record.DateTime) < TimeSpan.FromSeconds(0.01));
            Debug.Assert(r2.ModuleName == record.ModuleName);
            Debug.Assert(r2.Message == "message");
        }

        private static void TestLogger(Logger logger)
        {
            logger.Debug("This is a test message with {level}.", "Debug");
            logger.Info("This is a test message with {level}.", "Info");
            logger.Warn("This is a test message with {level}.", "Warn");
            logger.Error("This is a test message with {level}.", "Error");
            logger.Fatal("This is a test message with {level}.", "Fatal");
        }

        public static void TestWriteFile()
        {
            LogRecord record = LogRecord.Create(LogLevel.Fatal, "message");
            string directoryPath = @"C:\temp";
            LogManager.SetImplementation(new FileWriterLogManager(directoryPath,record.ModuleName));
            Logger logger = LogManager.GetLogger("");
            Parallel.For(0, 100, i => {
                logger.Debug("this is a debug test");
                logger.Info("this is a info test");
                logger.Warn("this is a warnning test");
                logger.Error("this is a error test");
                logger.Fatal("this is a fatal test");
            });
        }
    }
}
