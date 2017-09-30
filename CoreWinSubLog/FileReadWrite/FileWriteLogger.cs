using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FileWriteLogger : Logger
    {
        private TextFileReadWrite _fileWriter;
        private IFilePathHelper _filePathHelper;
        private readonly BlockingAction<LogRecord> _blockingAction;

        /// <summary>
        ///  Initializes an instance of the <see cref="FileWriteLogger"/>.
        /// </summary>
        /// <param name="fileWriter">the file writer class</param>
        protected internal FileWriteLogger(string directoryPath,string modelName=null)
        {
            string moduleName = modelName ?? Process.GetCurrentProcess().ProcessName;
            _filePathHelper = new NewFileWithSizeHelper(directoryPath, moduleName);
            _fileWriter = new TextFileReadWrite(_filePathHelper.GetFilePath());
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
            LogRecord record = LogRecord.Create(level, content);
            _blockingAction.Post(record);
        }

        public void Log(LogRecord record)
        {
            _blockingAction.Post(record);
        }

        private void WriteLog(LogRecord record)
        {
            if (_filePathHelper.NewFileOrDefualt())
            {
                _fileWriter = new TextFileReadWrite(_filePathHelper.GetFilePath());
            }
            _fileWriter.Write(record);
        }
    }

    public class FileWriterLogManager:LogManager
    {
        private readonly Logger _loggerImpl;

        public FileWriterLogManager(string directoryPath, string modelName=null)
        {
            _loggerImpl = new FileWriteLogger(directoryPath, modelName);
        }

        protected override Logger GetLoggerImpl(string name)
        {
             return _loggerImpl;
        }
    }
}
