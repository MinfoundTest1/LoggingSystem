using System;
using System.Diagnostics;

namespace CoreWinSubLog
{
    public class LogRecordFactory
    {
        private static string _processName;
        public static string ModuleName
        {
            get { return _processName ?? (_processName = Process.GetCurrentProcess().ProcessName); }
        }

        /// <summary>
        /// Create a log record with current time and current process name.
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Log message</param>
        /// <returns></returns>
        public static LogRecord Create(LogLevel level, string message)
        {
            DateTime currentTime = DateTime.Now;
            return new LogRecord(level, currentTime, ModuleName, message);
        }

    }
}
