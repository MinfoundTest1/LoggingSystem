using CoreWinSubLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingSystemTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            //    LoggerTest.TestNullLogger();
            //    Console.WriteLine();
            //    LoggerTest.TestConsoleLogger();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //Console.WriteLine();
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();

            TextFileReadWrite write = new TextFileReadWrite(@"C:\Users\xingchen.shen\Desktop\1.txt", "DBService");
            TextFileReadWrite read1 = new TextFileReadWrite(@"C:\Users\xingchen.shen\Desktop\1.txt", "DBService");
            Thread writer = new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    write.Write("this is a write test" + i, LogLevel.Debug, DateTime.Now);
                    Console.WriteLine("Write the info" + i);
                }
                Console.WriteLine("Write OK");
            });
            writer.Start();

            Thread read = new Thread(() =>
            {
                while (true)
                {
                    string message = read1.ReadLine();
                    if (message != null)
                    {
                        Console.WriteLine(message);
                    }

                }
            });
            read.Start();
            Console.ReadLine();
        }
    }
}
