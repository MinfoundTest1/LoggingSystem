using CoreWinSubLog;
using CoreWinSubLogService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            Parallel.For(0, 50000, t =>
            {
                TestLogger(logger);
            });
        }

        public static void TestWcfLoggerClient()
        {
            LogManager.SetImplementation(new WcfLoggerManager("127.0.0.1"));
            Logger logger = LogManager.GetLogger("");
            for (int i = 0; i < 60000; i++)
            {
                TestLogger(logger);
            }
        }

        public static void TestMinLogger()
        {
            LogManager.SetImplementation(new MinLoggerManager("127.0.0.1", "C:\\Temp\\Log"));
            Logger logger = LogManager.GetLogger("");
            Parallel.For(0, 2000, t =>
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

        //public static void TestLogRecordString()
        //{
        //    LogRecord record = LogRecordFactory.Create(LogLevel.Fatal, "message");
        //    string recordString = record.ToString();
        //    LogRecord r2 = LogRecord.FromString(recordString);
        //    r2.ResetModuleName(Process.GetCurrentProcess().ProcessName);

        //    Debug.Assert(r2.Level == LogLevel.Fatal);
        //    Debug.Assert((r2.DateTime - record.DateTime) < TimeSpan.FromSeconds(0.01));
        //    Debug.Assert(r2.ModuleName == record.ModuleName);
        //    Debug.Assert(r2.Message == "message");
        //}

        private static void TestLogger(Logger logger)
        {
            logger.Debug("This is a test message with {0}.", "Debug");
            logger.Info("This is a test message with {0}.", "Info");
            logger.Warn("This is a test message with {0}.", "Warn");
            logger.Error("This is a test message with {0}.", "Error");
            logger.Fatal("This is a test message with {0}.", "Fatal");
        }

        public static void TestFileWriteLogger()
        {
            
            LogRecord record = LogRecordFactory.Create(LogLevel.Fatal, "message");
            string directoryPath = @"C:\temp";
            LogManager.SetImplementation(new FileWriterLogManager(directoryPath, record.ModuleName));
            Logger logger = LogManager.GetLogger("");
            Console.WriteLine(record.ModuleName);
            for (int i = 0; i < 40000; i++)
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
            Parallel.For(0, 60000, i =>
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
            string path = @"C:\temp\LoggingSystemTest\20171009_102217_3.txt";
            if (!File.Exists(path))
            {
                Console.WriteLine("No File");
                return;
            }
            TextFileReadWrite read = new TextFileReadWrite(path);
            while (true)
            {
                Console.WriteLine(read.DeleteFirstLine().Message);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

        }
        public static void TestReadAllFile()
        {
            string path = @"C:\temp\LoggingSystemTest\20171009_165554_1.txt";
            if (!File.Exists(path))
            {
                Console.WriteLine("No File");
                return;
            }
            TextFileReadWrite read = new TextFileReadWrite(path);
            string message = string.Empty;
            bool isend = false;
            int index = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine("正在读取，请稍后……");
            do
            {
                index++;
                isend = read.ReadLine(ref message);
            }
            while (!isend);
            watch.Stop();
            Console.WriteLine("End:" + watch.ElapsedMilliseconds);
          
        }

        //public static void TestReadMuiltFile()
        //{
        //    LogFileReader reader = new LogFileReader(@"C:\temp\Log");
        //    List<LogRecord> records = new List<LogRecord>();
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    reader.ReadAllFileRecord(ref records);
        //    watch.Stop();
        //    Console.WriteLine("共有数据{0}条  共耗时{1}",records.Count(),watch.ElapsedMilliseconds);
        //}

        public static void TestReadLine()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string path = @"C:\temp\LoggingSystemTest";
            if (!Directory.Exists(path))
            {
                Console.WriteLine("No Dir");
                return;
            }
            ILogTextReader reader = new LogTextReader(path);
            List<LogRecord> records = reader.ReadAllLogRecords() as List<LogRecord>;
            //reader.ReadAllLogRecords();
            watch.Stop();
            Console.WriteLine("已读完，数量：{0},时间：{1}", records.Count(),watch.ElapsedMilliseconds);
        }

        //public static void TestQuerySelect()
        //{
        //    LogFileReader reader = new LogFileReader(@"C:\temp");
        //    List<LogRecord> records = new List<LogRecord>();
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    reader.ReadAllFileRecord(ref records);
        //    List<LogRecord> results = new List<LogRecord>();
        //    //results = records.Where(s => s.ModuleName == "LoggingSystemTest").ToList();
        //    //results = records.Where(s => s.Level == LogLevel.Debug).ToList();
        //    //results = records.Where(s => s.DateTime.Year == 2017 && s.DateTime.Month == 10 && s.DateTime.Day == 9).ToList();
        //    watch.Stop();
        //    Console.WriteLine("查询到的数据量：{0},共耗时{1}",results.Count(),watch.ElapsedMilliseconds);
        //}


        public static void TestInsertIntoSql()
        {
            //SqlWriteLogger write = new SqlWriteLogger();
            //Parallel.For(0, 200000, i =>
            //{
            //    LogRecord recordFatal = LogRecordFactory.Create(LogLevel.Fatal, string.Format("{0} wirte this is a test message with {1}", Task.CurrentId, "Fatal"));
            //    write.WriteToSql(Transform(recordFatal));

            //    LogRecord recordDebug = LogRecordFactory.Create(LogLevel.Debug, string.Format("{0} wirte this is a test message with {1}", Task.CurrentId, "Debug"));
            //    write.WriteToSql(Transform(recordDebug));

            //    LogRecord recordInfo = LogRecordFactory.Create(LogLevel.Info, string.Format("{0} wirte this is a test message with {1}", Task.CurrentId, "Info"));
            //    write.WriteToSql(Transform(recordInfo));

            //    LogRecord recordWarning = LogRecordFactory.Create(LogLevel.Warning, string.Format("{0} wirte this is a test message with {1}", Task.CurrentId, "Warning"));
            //    write.WriteToSql(Transform(recordWarning));
            //});

            //Console.WriteLine("OK");

        }

        private static  CoreWinSubDataLib.LogRecord Transform(LogRecord logRecord)
        {
            CoreWinSubDataLib.LogRecord record = new CoreWinSubDataLib.LogRecord();
            record.Epoch = logRecord.DateTime;
            record.Classification = record.Epoch.Millisecond.ToString();
            record.SubOrigin = logRecord.ModuleName;
            record.Severity = logRecord.Level.ToString();
            record.Message = logRecord.Message;
            return record;
        }

        //public static void TestReadLogFromSql()
        //{
        //    LogRepository _logRepository = new LogRepository();
        //    Console.WriteLine("正在读取……");
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    CoreWinSubDataLib.LogRecord[] records = _logRepository.QueryLogWithLimit(0,300000);
        //    watch.Stop();
        //    Console.WriteLine("cout:" + records.Count() + "Time:" + watch.ElapsedMilliseconds);
        //}

        //public static void TestWcfReadLogger()
        //{

        //    //LogManager.SetImplementation(new WcfLoggerManager("127.0.0.1"));
        //    Console.WriteLine("正在读取……");
        //    WcfLogger logger = new WcfLogger("127.0.0.1");
        //    LogRecord [] records= logger.QueryLogWithLimit(0,250000);
        //    Console.WriteLine(records.Count());

        //}

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
        public static void TestBatchAction()
        {
            Action<int[]> action = new Action<int[]>(array =>
            {
                string s = string.Empty;
                foreach (var item in array)
                {
                    s += $" {item}";
                }
                Console.WriteLine(s);
            });

            BatchAction<int> batchAction = new BatchAction<int>(action, 10);
            batchAction.Batch(Enumerable.Range(1, 10003));
        }

        public static void TestTextFileWrite()
        {
            string filename = "C:\\Temp\\Log\\1.txt";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (File.Create(filename))
            { }
            
            TextFileReadWrite textFileReadWrite = new TextFileReadWrite(filename);
            //// Write module name 10 times. The result should be only one module name.
            //Enumerable.Range(0, 10).ToList().ForEach(i => textFileReadWrite.WriteModuleName(Process.GetCurrentProcess().ProcessName));

            Parallel.ForEach(Enumerable.Range(0, 10), i =>
            {
                LogRecord recordFatal = LogRecordFactory.Create(LogLevel.Fatal, $"{Task.CurrentId} wirte this is a test message {i} with Fatal");
                textFileReadWrite.WriteLogLine(recordFatal);
            });

            // Write module name 10 times after the log records inserted. 
            Enumerable.Range(0, 10).ToList().ForEach(i => textFileReadWrite.WriteModuleName(Process.GetCurrentProcess().ProcessName));
        }
    }
}
