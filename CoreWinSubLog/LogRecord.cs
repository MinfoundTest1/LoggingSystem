using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace CoreWinSubLog
{
    /// <summary>
    /// Value object to deal with the log as record.
    /// </summary>
    [DataContract]
    public class LogRecord
    {
        [DataMember]
        public int LogId { get; private set; }

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
        /// Create a log record with current time and current process name.
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

        public static LogRecord NullRecord()
        {
            return new LogRecord(LogLevel.Debug, DateTime.MinValue, "Default", "Null");
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
        /// Get log string
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LogRecord FromString(string content)
        {
            const int len = 5;
            string[] msg = content.Split(new string[] { "] [" }, len, StringSplitOptions.None);
            if (msg.Count() >= len - 1)
            {
                LogLevel level = LogLevel.Debug;
                Enum.TryParse(msg[1], out level);

                DateTime dateTime;
                DateTime.TryParseExact(msg[2], "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);

                string mouduleName = msg[3];
                string message = (msg.Length == len) ? msg[4].Substring(0, msg[4].Length - 1) : "";

                LogRecord logRecord = new LogRecord(level, dateTime, mouduleName, message);
                int id = 0;
                int.TryParse(msg[0].Substring(1), out id);
                logRecord.LogId = id;
                return logRecord;
            }
            else
            {
                return NullRecord();
            }
        }

        /// <summary>
        /// Convert log record to string (DateTime ("yyyy-MM-dd HH:mm:ss.fff")).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"[{LogId}] [{Level}] [{DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture)}] [{ModuleName}] [{Message}]");
        }
    }
}
