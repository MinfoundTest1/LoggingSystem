using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    /// <summary>
    /// Logger for output to a <see cref="WcfLogger"/> firstly, 
    /// if any problem, output to textLogger.
    /// /// </summary>
    public class MinLogger : Logger
    {
        private readonly WcfLogger _wcfLogger;
        private Logger _localLogger;

        /// <summary>
        /// Initializes an instance with host address and file path.
        /// </summary>
        /// <param name="hostIPAddress">WCF host ip address</param>
        /// <param name="filePath">Text file directory</param>
        public MinLogger(string hostIPAddress, string filePath)
        {
            _wcfLogger = new WcfLogger(hostIPAddress);
        }

        public override void Log(LogLevel level, string message, params object[] args)
        {
            Logger logger = GetValidLogger();
            logger.Log(level, message, args);
        }

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
    }
}
