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
<<<<<<< HEAD
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
=======
            try
            {
                LoggerTest.TestNullLogger();
                Console.WriteLine();
                LoggerTest.TestConsoleLogger();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
>>>>>>> parent of 6ad426b... Add TextReadWrite Class
        }
    }
}
