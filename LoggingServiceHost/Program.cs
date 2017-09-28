using CoreWinSubLog;
using CoreWinSubLogService;
using System;
using System.ServiceModel;

namespace LoggingServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(LogService));
            try
            {             
                host.AddServiceEndpoint(typeof(ILogService), LogServiceBinding.TcpBinding(), LogServiceBinding.Uri("127.0.0.1"));
                host.Open();
                Console.WriteLine("Log service is running...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
