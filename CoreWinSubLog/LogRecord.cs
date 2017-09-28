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
        /// Get log string
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LogRecord FromString(string content)
        {
            string[] msg = content.Split(new string[] { "] [" }, 4, StringSplitOptions.None);
            if (msg.Count() >= 3)
            { 
                LogLevel level = LogLevel.Debug;
                Enum.TryParse(msg[0].Substring(1), out level);

                DateTime dateTime;
                DateTime.TryParseExact(msg[1], "yy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);
                if (msg.Length == 4)
                {
                    return new LogRecord(level, dateTime, msg[2], msg[3].Substring(0, msg[3].Length - 1));
                }
                else
                {
                    return new LogRecord(level, dateTime, msg[2], "");
                }
            }
            else
            {
                return NullRecord();
            }
        }

        /// <summary>
        /// Convert log record to string (DateTime ("yy-MM-dd HH:mm:ss.fff")).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"[{Level}] [{DateTime.ToString("yy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture)}] [{ModuleName}] [{Message}]");
        }
    }
}
