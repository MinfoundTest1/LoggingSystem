using System.ServiceModel;

namespace CoreWinSubLog
{
    /// <summary>
    /// Log service interface to output logs.
    /// </summary>
    [ServiceContract]
    public interface ILogService
    {
        /// <summary>
        /// Save a log record.
        /// </summary>
        /// <param name="logRecord">Single log record</param>
        [OperationContract(IsOneWay = true, Name = "LogSingle")]
        void Log(LogRecord logRecord);

        /// <summary>
        /// Save log record array.
        /// </summary>
        /// <param name="logRecords">Log record array</param>
        [OperationContract(IsOneWay = true, Name = "LogArrary")]
        void Log(LogRecord[] logRecords);
    }
}
