using System;
using System.Collections.Concurrent;
using System.ServiceModel;

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

        /// <summary>
        /// Initializes an instance with given WCF service ip.
        /// </summary>
        /// <param name="ipAddress">WCF service ip address</param>
        public WcfLogger(string ipAddress)
        {
            _remoteAddress = ipAddress;
            _logService = new LogClient(ipAddress);
            _blockingAction = new BlockingAction<LogRecord>(r => TryLog(r));
        }

        /// <summary>
        /// Initializes an instance with given WCF service ip, and a back-up action for the logs when WCF connection is broken.
        /// </summary>
        /// <param name="ipAddress">WCF service ip Address</param>
        /// <param name="backupAction">Action for the logs don't pass the conenction</param>
        internal WcfLogger(string ipAddress, Action<LogRecord> backupAction)
        {
            _remoteAddress = ipAddress;
            _logService = new LogClient(ipAddress);
            _blockingAction = new BlockingAction<LogRecord>(r => TryLog(r));
            _backupAction = backupAction;
        }

        public override void Log(LogLevel level, string msg, params object[] args)
        {
            string content = string.Format(NameFormatToPositionalFormat(msg), args);
            LogRecord record = LogRecord.Create(level, content);
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
                return (client.State != CommunicationState.Closed) && (client.State != CommunicationState.Faulted) && (client.State != CommunicationState.Closing);
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
            catch (Exception)
            {
                // If the garbage collection 
                _backupAction?.Invoke(record);
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
