using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FileWriteLogger : Logger
    {
        private TextFileReadWrite _fileWriter;
        private FilePathHelper _filePathHelper;
        private readonly BlockingAction<LogRecord> _blockingAction;

        /// <summary>
        ///  Initializes an instance of the <see cref="FileWriteLogger"/>.
        /// </summary>
        /// <param name="fileWriter">the file writer class</param>
        protected internal FileWriteLogger(string directoryPath,string modelName)
        {
            _filePathHelper = new FilePathHelper(directoryPath, modelName);
            _fileWriter = new TextFileReadWrite(_filePathHelper.FilePath);
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

        private void WriteLog(LogRecord record)
        {
            _filePathHelper.NewFileOrDefualt(NewFileOpations.FileSize);
            _fileWriter.SetFilePath(_filePathHelper.FilePath);
            _fileWriter.Write(record);
        }
    }

    public class FileWriterLogManager:LogManager
    {
        private readonly Logger _loggerImpl;

        public FileWriterLogManager(string directory,string modelName)
        {
            _loggerImpl = new FileWriteLogger(directory,modelName);
        }

        protected override Logger GetLoggerImpl(string name)
        {
             return _loggerImpl;
        }
    }
}
