using CoreWinSubLog;
using System;
using System.Collections.Generic;
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

            TextFileReadWrite read = new TextFileReadWrite(@"C:\Users\xingchen.shen\Desktop\1.txt","DBService");
            TextFileReadWrite write = new TextFileReadWrite(@"C:\Users\xingchen.shen\Desktop\1.txt", "DBService");
            //while (true)
            //{
            //    string msg = read.ReadLine();
            //    write.Write(DateTime.Now,LogLevel.Debug,"this is a test");
            //    //if (msg != null)
            //    //{
            //    //    Console.WriteLine(msg);
            //    //}
            //}

            for (int i = 0; i < 1000; i++)
            {
                //write.Write(DateTime.Now,LogLevel.Debug,"this is a test "+i);
                string msg = read.ReadLine();
                if (msg != null)
                {
                    Console.WriteLine(msg);
                }

            }
            Console.WriteLine("Write ok!");
            Console.ReadKey();
        }
    }
}
