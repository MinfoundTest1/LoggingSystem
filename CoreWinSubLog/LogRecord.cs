using System;
using System.Globalization;
using System.Linq;
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
        /// Log id in database
        /// </summary>
        [DataMember]
        public int LogId { get; protected set; }

        /// <summary>
        /// Log level
        /// </summary>
        [DataMember]
        public LogLevel Level { get; protected set; }

        /// <summary>
        /// DateTime when log create
        /// </summary>
        [DataMember]
        public DateTime DateTime { get; protected set; }

        /// <summary>
        /// Name of module (Process) 
        /// </summary>
        [DataMember]
        public string ModuleName { get; protected set; }

        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; protected set; }

        public LogRecord(LogLevel level, DateTime dateTime, string moduleName, string message)
        {
            Level = level;
            DateTime = dateTime;
            ModuleName = moduleName;
            Message = message;
        }

        /// <summary>
        /// Get the null record.
        /// </summary>
        /// <returns>Default log record</returns>
        public static LogRecord NullRecord()
        {
            return new LogRecord(LogLevel.Debug, DateTime.MinValue, "Null", "Null");
        }
       
        /// <summary>
        /// Check if this log record is null record.
        /// </summary>
        /// <returns></returns>
        public bool IsNull()
        {
            return (DateTime == DateTime.MinValue) && (ModuleName == "Null");
        }

        /// <summary>
        /// Reset the log id.
        /// </summary>
        /// <param name="id">Log Id</param>
        public void ResetId(int id)
        {
            LogId = id;
        }

        /// <summary>
        /// Convert log record to string (DateTime ("yyyy-MM-dd HH:mm:ss.fff")).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"{DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture)} [{Level}] {Message}");
        }
    }
}
