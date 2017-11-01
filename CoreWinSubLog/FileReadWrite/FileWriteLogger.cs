using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FileWriteLogger : Logger
    {
        private ILogTextWriter _logTextWriter;//write log
        private FilePathHelper _filePathHelper;//to create new log file or defualt
        private readonly BlockingAction<LogRecord> _blockingAction;
        /// <summary>
        ///  Initializes an instance of the <see cref="FileWriteLogger"/>.
        /// </summary>
        /// <param name="fileWriter">the file writer class</param>
        protected internal FileWriteLogger(string directoryPath)
        {
            _filePathHelper = new NewFileWithTimeHelper(directoryPath);
            _blockingAction = new BlockingAction<LogRecord>(r => WriteLog(r));
        }

        /// <summary>
        /// write the log
        /// </summary>
        /// <param name="level">the log level</param>
        /// <param name="message">the log message</param>
        /// <param name="args">the log params</param>
        public override void Log(LogLevel level, string message, params object[] args)
        {
            string content = string.Format(NameFormatToPositionalFormat(message), args);
            LogRecord record = LogRecordFactory.Create(level, content);
            _blockingAction.Post(record);
        }

        /// <summary>
        /// write log
        /// </summary>
        /// <param name="record"></param>
        public void Log(LogRecord record)
        {
            _blockingAction.Post(record);
        }

        private void WriteLog(LogRecord record)
        {
            try
            {
                string fileName = string.Empty;
                if (_filePathHelper.CreateNewOrDefualt(ref fileName) || _logTextWriter == null)
                {
                    _logTextWriter = new TextFileReadWrite(fileName);
                    _logTextWriter.WriteModuleName(record.ModuleName);
                }

                _logTextWriter.WriteLogLine(record);
            }
            catch
            {

            }

        }
    }

    public class FileWriterLogManager : LogManager
    {
        private readonly Logger _loggerImpl;
        private readonly LogAutoRemover _txtLogRemove;

        /// <summary>
        /// init file write manager class
        /// </summary>
        /// <param name="directoryPath"> the log total directory, or accurate to sub folder</param>
        /// <param name="keepDays">the log keep days</param>
        public FileWriterLogManager(string directoryPath, int keepDays)
        {
            _loggerImpl = new FileWriteLogger(directoryPath);
            _txtLogRemove = TextLogAutoRemover.Create(keepDays, directoryPath);
        }

        protected override Logger GetLoggerImpl(string name)
        {
            return _loggerImpl;
        }
    }
}
