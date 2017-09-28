using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace CoreWinSubLog
{
    /// <summary>
    /// Value object to deal with the log as record.
    /// </summary>
    [DataContract]
    public class LogRecord
    {
        /// <summary>
        /// Log level
        /// </summary>
        [DataMember]
        public LogLevel Level { get; private set; }

        /// <summary>
        /// DateTime when log create
        /// </summary>
        [DataMember]
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Name of module (Process) 
        /// </summary>
        [DataMember]
        public string ModuleName { get; private set; }

        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; private set; }

        public LogRecord(LogLevel level, DateTime dateTime, string moduleName, string message)
        {
            Level = level;
            DateTime = dateTime;
            ModuleName = moduleName;
            Message = message;
        }

        /// <summary>
        /// Create log record with current time and current process name.
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Log message</param>
        /// <returns></returns>
        public static LogRecord Create(LogLevel level, string message)
        {
            DateTime currentTime = DateTime.Now;
            string moduleName = Process.GetCurrentProcess().ProcessName;
            return new LogRecord(level, currentTime, moduleName, message);
        }
    }
}
