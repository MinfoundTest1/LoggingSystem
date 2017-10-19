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

        ///// <summary>
        ///// Reset the module name.
        ///// </summary>
        ///// <param name="moduleName"></param>
        //public void ResetModuleName(string moduleName)
        //{
        //    ModuleName = moduleName;
        //}

        //Commit by Yuqing as the string doesn't contain an integral information of log record.
        /// <summary>
        /// Get a log record form given string
        /// </summary>
        /// <param name="content">string matched to a log record</param>
        /// <returns></returns>
        public static LogRecord FromString(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return NullRecord();
            }

            string timeString = content.Substring(0, 23);
            DateTime dateTime;
            DateTime.TryParseExact(timeString, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);

            string otherString = content.Substring(24);

            LogLevel logLevel = LogLevel.Info;
            string theMessage = string.Empty;
            if (otherString.StartsWith("["))
            {
                string[] msgs = otherString.Split(new string[] { " " }, 2, StringSplitOptions.None);
                string levelString = msgs[0].TrimStart('[').TrimEnd(']');
                Enum.TryParse(levelString, out logLevel);
                theMessage = msgs[1];
            }
            else
            {
                theMessage = otherString;
            }

            return new LogRecord(logLevel, dateTime, "", theMessage);
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
