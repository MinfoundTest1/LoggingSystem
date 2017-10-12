using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreWinSubLog
{
    /// <summary>
    /// Log client of WCF connection.
    /// </summary>
    class LogClientBase : ClientBase<ILogService>, ILogService
    {
        public LogClientBase(Binding binding, EndpointAddress endpointAddress)
            : base(binding, endpointAddress)
        {
        }

        public void Log(LogRecord logRecord)
        {
            Channel.Log(logRecord);
        }

        public void Log(LogRecord[] logRecords)
        {
            Channel.Log(logRecords);
        }
    }

    /// <summary>
    /// Log client of WCF with Tcp connection.
    /// </summary>
    class LogTcpClient : LogClientBase
    {
        public LogTcpClient(Binding binding, string ipAddress)
            : base(binding, new EndpointAddress(LogServiceBinding.TcpUri(ipAddress)))
        {
        }

        public LogTcpClient(string ipAddress)
            : this(LogServiceBinding.TcpBinding(), ipAddress)
        { }
    }

    /// <summary>
    /// Log client of WCF with Http connection.
    /// </summary>
    class LogHttpClient : LogClientBase
    {
        public LogHttpClient(Binding binding, string ipAddress)
            : base(binding, new EndpointAddress(LogServiceBinding.HttpUri(ipAddress)))
        {
        }

        public LogHttpClient(string ipAddress)
            : this(LogServiceBinding.HttpBinding(), ipAddress)
        { }
    }
}
