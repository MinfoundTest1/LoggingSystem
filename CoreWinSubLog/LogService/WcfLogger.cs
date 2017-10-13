using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    /// <summary>
    /// Logger for output to a WCF service implmentation.
    /// </summary>
    public class WcfLogger : Logger
    {
        private readonly string _remoteAddress;
        private ILogService _logService;

        private readonly BlockingAction<LogRecord> _blockingAction;
        private readonly Action<LogRecord> _backupAction;

        private readonly int _timerPeriod = 30 * 1000;

        private readonly object _mutex = new object();

        /// <summary>
        /// Initializes an instance with given WCF service ip.
        /// </summary>
        /// <param name="ipAddress">WCF service ip address</param>
        public WcfLogger(string ipAddress)
        {
            _remoteAddress = ipAddress;
            _logService = new LogHttpClient(ipAddress);
            _blockingAction = new BlockingAction<LogRecord>(r => TryLog(r));
        }

        /// <summary>
        /// Initializes an instance with given WCF service ip, and a back-up action for the logs when WCF connection is broken.
        /// </summary>
        /// <param name="ipAddress">WCF service ip Address</param>
        /// <param name="backupAction">Action for the logs which don't pass the conenction</param>
        internal WcfLogger(string ipAddress, Action<LogRecord> backupAction)
            : this(ipAddress)
        {
            _backupAction = backupAction;
        }

        public override void Log(LogLevel level, string msg, params object[] args)
        {
            string content = string.Format(NameFormatToPositionalFormat(msg), args);
            LogRecord record = LogRecordFactory.Create(level, content);
            _blockingAction.Post(record);
        }

        public void Log(LogRecord logRecord)
        {
            _blockingAction.Post(logRecord);
        }

        /// <summary>
        /// Check if the WCF connection is valid.
        /// </summary>
        public bool IsValid()
        {
            var client = _logService as ClientBase<ILogService>;
            if (client != null)
            {
                return (client.State != CommunicationState.Closed) && (client.State != CommunicationState.Faulted);
            }
            return false;
        }

        /// <summary>
        /// Try log the record by log service.
        /// </summary>
        private void TryLog(LogRecord record)
        {
            try
            {
                _logService.Log(record);
            }
            catch (CommunicationException)
            {
                var client = _logService as ClientBase<ILogService>;
                if (client != null)
                {
                    lock (client)
                    {
                        if (client.State != CommunicationState.Closed || client.State != CommunicationState.Closing)
                        {
                            client.Close();
                            // Try re-connect the service.
                            AutoReconnectionAsync();
                        }                       
                    }
                }
                _backupAction?.Invoke(record);
            }
            catch (Exception)
            {
                _backupAction?.Invoke(record);
            }
        }

        /// <summary>
        /// Re-connect the WCF service using timer. 
        /// </summary>
        /// <returns></returns>
        private Task AutoReconnectionAsync()
        {
            return Task.Run(() =>
            {
                // event to notice the service connected
                var autoEvent = new AutoResetEvent(false);
                // New timer to re-connect the service.
                Timer reconnectTimer = new Timer(tryReconnect, autoEvent, 1000, _timerPeriod);   
                // Wait connected event.
                autoEvent.WaitOne();
                Console.WriteLine("Reconnect");
                reconnectTimer.Dispose();
            });
        }

        /// <summary>
        /// Try re-connect to the WCF service.
        /// </summary>
        /// <param name="o"></param>
        private void tryReconnect(object o)
        {
            var client = new LogHttpClient(_remoteAddress);
            try
            {
                Console.WriteLine("Timer is running...");
                AutoResetEvent auto = (AutoResetEvent)o;
                // Try log to service.
                client.Log(LogRecord.NullRecord());
                // If no exception, reset the _logService.
                lock(_mutex)
                {
                    _logService = client;
                }
                // Set the event.
                auto.Set();
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// Manager for logging to a <see cref="WcfLogger"/> implementation.
    /// </summary>
    public class WcfLoggerManager : LogManager
    {
        readonly Logger _loggerImpl;

        /// <summary>
        /// Initializes an instance to WCF service.
        /// </summary>
        /// <param name="ipAddress">WCF service ip address</param>
        public WcfLoggerManager(string ipAddress)
        {
            _loggerImpl = new WcfLogger(ipAddress);
        }

        /// <summary>
        /// Get logger from the current log manager implementation.
        /// </summary>
        /// <param name="name">Classifier name, typically namespace or type name.</param>
        /// <returns>Logger from the current log manager implementation.</returns>
        protected override Logger GetLoggerImpl(string name)
        {
            return _loggerImpl;
        }
    }

}
