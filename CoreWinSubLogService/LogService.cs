using CoreWinSubLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace CoreWinSubLogService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class LogService : ILogService
    {
        // Action block to save the record array to database.
        static ActionBlock<CoreWinSubDataLib.LogRecord> _actionBlock = new ActionBlock<CoreWinSubDataLib.LogRecord>(t => Store(t), new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });
        static LogRepository _logRepository = new LogRepository();

        public void Log(LogRecord logRecord)
        {
            _actionBlock.Post(Transform(logRecord));
        }

        public void Log(LogRecord[] logRecords)
        {
            foreach (var item in logRecords)
            {
                _actionBlock.Post(Transform(item));
            }
        }

        //static DateTime _firstTime = DateTime.MinValue;
        //static ActionBlock<DateTime> _testBlock = new ActionBlock<DateTime>(t =>
        //{
        //    if (_firstTime == DateTime.MinValue)
        //        _firstTime = t;
        //    Console.WriteLine((t - _firstTime).Seconds);
        //});
        private static void Store(CoreWinSubDataLib.LogRecord logRecord)
        {
            _logRepository.Save(logRecord);
  //          _testBlock.Post(DateTime.Now);
        }

        /// <summary>
        /// Translate the CoreWinSubLog.LogRecord to CoreWinSubDataLib.LogRecord.
        /// </summary>
        private CoreWinSubDataLib.LogRecord Transform(LogRecord logRecord)
        {
            CoreWinSubDataLib.LogRecord record = new CoreWinSubDataLib.LogRecord();
            record.Epoch = logRecord.DateTime;
            record.Classification = record.Epoch.Millisecond.ToString();
            record.SubOrigin = logRecord.ModuleName;
            record.Severity = logRecord.Level.ToString();
            record.Message = logRecord.Message;
            return record;
        }
    }
}
