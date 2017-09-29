using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreWinSubLog
{
    /// <summary>
    /// Log client of WCF connection.
    /// </summary>
    class LogClient : ClientBase<ILogService>, ILogService
    {
        public LogClient(Binding binding, string ipAddress)
            : base(binding, new EndpointAddress(LogServiceBinding.Uri(ipAddress)))
        {
        }

        public LogClient(string ipAddress)
            : this(LogServiceBinding.TcpBinding(), ipAddress)
        { }

        public void Log(LogRecord logRecord)
        {
            Channel.Log(logRecord);
        }

        public void Log(LogRecord[] logRecords)
        {
            Channel.Log(logRecords);
        }
    }
}
