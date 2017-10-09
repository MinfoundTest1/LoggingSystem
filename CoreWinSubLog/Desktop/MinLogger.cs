
namespace CoreWinSubLog
{
    /// <summary>
    /// Logger for output to a <see cref="WcfLogger"/> firstly, 
    /// if any problem, output to textLogger.
    /// /// </summary>
    public class MinLogger : Logger
    {
        // Logger to WCF service.
        private readonly WcfLogger _wcfLogger;
        // Logger to local file
        private readonly FileWriteLogger _localLogger;

        /// <summary>
        /// Initializes an instance with host address and file path.
        /// </summary>
        /// <param name="hostIPAddress">WCF host ip address</param>
        /// <param name="filePath">Text file directory</param>
        public MinLogger(string hostIPAddress, string filePath)
        {
            _localLogger = new FileWriteLogger(filePath);
            _wcfLogger = new WcfLogger(hostIPAddress, r => LogToLocal(r));
        }

        public override void Log(LogLevel level, string message, params object[] args)
        {
            Logger logger = GetValidLogger();
            logger.Log(level, message, args);
        }

        /// <summary>
        /// Get one valid logger. (WCFLogger > LocalLogger)
        /// </summary>
        /// <returns></returns>
        private Logger GetValidLogger()
        {
            if (_wcfLogger.IsValid())
            {
                return _wcfLogger;
            }
            else
            {
                return _localLogger;
            }
        }

        private void LogToLocal(LogRecord logRecord)
        {
            _localLogger.Log(logRecord);
        }
    }

    /// <summary>
    /// Manager for logging to a <see cref="MinLogger"/> implementation.
    /// </summary>
    public class MinLoggerManager : LogManager
    {
        readonly Logger _loggerImpl;

        /// <summary>
        /// Initializes an instance for MinLogger.
        /// </summary>
        /// <param name="ipAddress">WCF service ip address</param>
        public MinLoggerManager(string ipAddress, string filePath)
        {
            _loggerImpl = new MinLogger(ipAddress, filePath);
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
