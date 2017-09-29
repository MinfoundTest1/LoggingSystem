using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FileWriteLogger : Logger
    {
        private readonly TextFileReadWrite _fileWriter;

        /// <summary>
        ///  Initializes an instance of the <see cref="TextFileReadWrite"/>.
        /// </summary>
        /// <param name="fileWriter">the file writer class</param>
        protected internal FileWriteLogger(TextFileReadWrite fileWriter)
        {
            _fileWriter = fileWriter;
        }

        public override void Log(LogLevel level, string message, params object[] args)
        {
            string content = string.Format(NameFormatToPositionalFormat(message), args);
            LogRecord record = LogRecord.Create(level, content);
            _fileWriter.Write(record);
        }
    }

    public class FileWriterLogManager:LogManager
    {
        private readonly Logger _loggerImpl;

        public FileWriterLogManager(string directory,string modelName)
        {
            TextFileReadWrite fileWriter = new TextFileReadWrite(directory, modelName);
            fileWriter.CreateFilePath();
            _loggerImpl = new FileWriteLogger(fileWriter);
        }

        protected override Logger GetLoggerImpl(string name)
        {
             return _loggerImpl;
        }
    }
}
