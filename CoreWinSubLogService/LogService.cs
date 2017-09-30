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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class LogService : ILogService
    {
        // Batch block to batch the records and then insert to database.
        static BatchBlock<CoreWinSubDataLib.LogRecord> _batchBlock;
        // Action block to save the record array to database.
        static ActionBlock<CoreWinSubDataLib.LogRecord[]> _actionBlock = new ActionBlock<CoreWinSubDataLib.LogRecord[]>(t => _logRepository.Save(t));

        static LogRepository _logRepository = new LogRepository();

        IDisposable _link;
        static readonly object _mutex = new object();

        public LogService()
        {
            if (_link == null)
            {
                lock (_mutex)
                {
                    // Insert to database every 2 records.
                    _batchBlock = new BatchBlock<CoreWinSubDataLib.LogRecord>(2);
                    _link = _batchBlock.LinkTo(_actionBlock);
                }
            }
        }

        public void Log(LogRecord logRecord)
        {
            _batchBlock.Post(Transform(logRecord));
        }

        public void Log(LogRecord[] logRecords)
        {
            _actionBlock.Post(logRecords.Select(r => Transform(r)).ToArray());               
        }

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
