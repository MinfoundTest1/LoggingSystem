using CoreWinSubLog;
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
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //LoggerTest.TestNullLogger();
                //Console.WriteLine();
                //LoggerTest.TestConsoleLogger();
                //LoggerTest.TestWcfLogger();
                //LoggerTest.TestLogRecordString();
                // LoggerTest.TestFileWriteLogger();
                //LoggerTest.TestAsyWriteFile();
                //LoggerTest.TestDeleteFirstLine();s
                //  LoggerTest.TestMinLogger();
                //   LoggerTest.TestBatchAction();
                //   LoggerTest.TestTextFileWrite();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
