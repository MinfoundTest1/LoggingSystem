using CoreWinSubLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            Parallel.For(0, 100, t =>
            {
                TestLogger(logger);
            });
        }

        public static void TestMinLogger()
        {
            LogManager.SetImplementation(new MinLoggerManager("127.0.0.1", "C:\\Temp\\Log"));
            Logger logger = LogManager.GetLogger("");
            Parallel.For(0, 10000, t =>
            {
                TestLogger(logger);
            });
        }

        public static void TestFormat()
        {
            Logger logger = LogManager.GetLogger("");

            using (TimeIt test = new TimeIt("Fortmat 10000 times"))
            {
                for (int i = 0; i < 10000; i++)
                {
                    TestLogger(logger);
                }
            }
        }

        public static void TestLogRecordString()
        {
            LogRecord record = LogRecordFactory.Create(LogLevel.Fatal, "message");
            string recordString = record.ToString();
            LogRecord r2 = LogRecord.FromString(recordString);
            Debug.Assert(r2.Level == LogLevel.Fatal);
            Debug.Assert((r2.DateTime - record.DateTime) < TimeSpan.FromSeconds(0.01));
            Debug.Assert(r2.ModuleName == record.ModuleName);
            Debug.Assert(r2.Message == "message");
        }

        private static void TestLogger(Logger logger)
        {
            logger.Debug("This is a test message with {0}.", "Debug");
            logger.Info("This is a test message with {0}.", "Info");
            logger.Warn("This is a test message with {0}.", "Warn");
            logger.Error("This is a test message with {0}.", "Error");
            logger.Fatal("This is a test message with {0}.", "Fatal");
        }

        public static void TestWriteFile()
        {
            LogRecord record = LogRecordFactory.Create(LogLevel.Fatal, "message");
            string directoryPath = @"C:\temp";
            LogManager.SetImplementation(new FileWriterLogManager(directoryPath, record.ModuleName));
            Logger logger = LogManager.GetLogger("");
            for (int i = 0; i < 10000; i++)
            {
                logger.Debug("This is a test message with {level}.", "Debug");
                logger.Info("This is a test message with {level}.", "Info");
                logger.Warn("This is a test message with {level}.", "Warn");
                logger.Error("This is a test message with {level}.", "Error");
                logger.Fatal("This is a test message with {level}.", "Fatal");
            }
        }

        public static void TestAsyWriteFile()
        {
            LogRecord record = LogRecordFactory.Create(LogLevel.Fatal, "message");
            string directoryPath = @"C:\temp";
            LogManager.SetImplementation(new FileWriterLogManager(directoryPath, record.ModuleName));
            Logger logger = LogManager.GetLogger("");
            Parallel.For(0, 10000, i =>
            {
                logger.Debug("{0} wirte this is a test message with {1}.",Task.CurrentId, "Debug");
                logger.Info("{0} wirte this is a test message with {1}.", Task.CurrentId, "Info");
                logger.Warn("{0} wirte this is a test message with {1}.", Task.CurrentId, "Warn");
                logger.Error("{0} wirte this is a test message with {1}.", Task.CurrentId, "Error");
                logger.Fatal("{0} wirte this is a test message with {1}.", Task.CurrentId, "Fatal");
            });
        }

        public static void TestDeleteFirstLine()
        {
            TextFileReadWrite read = new TextFileReadWrite(@"C:\temp\LoggingSystemTest.vshost\20170930_154523_5.txt");
            while (true)
            {
                Console.WriteLine(read.DeleteFirstLine().Message);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

        }

        public static void TestReadLine()
        {
            TextFileReadWrite read = new TextFileReadWrite(@"C:\temp\LoggingSystemTest.vshost\20170930_154523_5.txt");
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(read.ReadLine());
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }
        //public static void TestFileOpenTime()
        //{
        //    string path = @"C:\temp\LoggingSystemTest.vshost\20170929{1}.txt";
        //    TextFileReadWrite read = new TextFileReadWrite(path);
        //    Stopwatch wtch = new Stopwatch();
        //    wtch.Start();
        //    double size = read.GetFileSize(path);
        //    string msg = read.ReadLine();
        //    wtch.Stop();
        //    Console.WriteLine(wtch.ElapsedMilliseconds);

        //    //result:
        //}
    }
}
