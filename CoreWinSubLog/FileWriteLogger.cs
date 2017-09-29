using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FileWriteLogger : FileLogger
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loglevel">log level</param>
        /// <param name="time">the log create time</param>
        /// <param name="modelName">the log create source</param>
        /// <param name="msg">the log msg</param>
        /// <param name="args">the log args</param>
        public override void Log(LogLevel loglevel, DateTime time, string modelName,string msg,params object[] args)
        {
            LogRecord recoder = new LogRecord(loglevel,time,modelName,msg);
            _fileWriter.Write(recoder,args);
        }
    }

    public class FileWriterLogManager
    {
        private readonly FileLogger _loggerImpl;

        public FileWriterLogManager(string directory,string modelName)
        {
            TextFileReadWrite fileWriter = new TextFileReadWrite(directory, modelName);
            _loggerImpl = new FileWriteLogger(fileWriter);
        }

        protected  FileLogger GetLoggerImpl(string name)
        {
            return _loggerImpl;
        }
    }
}
