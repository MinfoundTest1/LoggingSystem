using CoreWinSubLog;
using CoreWinSubLogService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CoreLoggingService
{
    public partial class LoggingService : ServiceBase
    {
        private ServiceHost host = new ServiceHost(typeof(LogService));

        public LoggingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host.AddServiceEndpoint(typeof(ILogService), LogServiceBinding.TcpBinding(), LogServiceBinding.TcpUri("127.0.0.1"));

            // Add http end point.
            host.AddServiceEndpoint(typeof(ILogService), LogServiceBinding.HttpBinding(), LogServiceBinding.HttpUri("127.0.0.1"));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }
}
