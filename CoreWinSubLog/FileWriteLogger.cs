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
        private readonly TextFileReadWrite _fileWriter;
        private double _maxFileSize = 8;//M
        private double _maxFileHour = 2;//Hour
        private double _maxFileLogCout = 10000;
        private double _currentLogCout = 0;
        /// <summary>
        ///  Initializes an instance of the <see cref="FileWriteLogger"/>.
        /// </summary>
        /// <param name="fileWriter">the file writer class</param>
        protected internal FileWriteLogger(string directoryPath, string modelName = null)
        {
            string moduleName = modelName ?? Process.GetCurrentProcess().ProcessName;

            TextFileReadWrite fileWriter = new TextFileReadWrite(directoryPath, moduleName);
            _fileWriter = fileWriter;
            _fileWriter.CheckDirectory();
            _fileWriter.CheckFile();
            //_currentLogCout = _fileWriter.GetDefualtFileLogCout();// add for NewOrDefualtFileByLogCout
        }

        /// <summary>
        /// write the log
        /// </summary>
        /// <param name="level">the log level</param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public override void Log(LogLevel level, string message, params object[] args)
        {
            string content = string.Format(NameFormatToPositionalFormat(message), args);
            LogRecord record = LogRecord.Create(level, content);

            //The three method writes about the same speed abort 25-30k/s
            NewOrDefualtFileBySize();
            //NewOrDefualtFileByLogCout();
            //NewOrDefualtFileByHour();

            _fileWriter.Write(record);
        }

        /// <summary>
        /// Log the record.
        /// </summary>
        public void Log(LogRecord logRecord)
        {

        }

        /// <summary>
        /// create a new file when the cureent file size was than the max
        /// </summary>
        private void NewOrDefualtFileBySize()
        {
            double filesize = _fileWriter.GetFileSize(_fileWriter.FilePath);
            if (filesize > _maxFileSize)
            {
                _fileWriter.FilePath= _fileWriter.CreateNewFilePath();
            }
        }

        /// <summary>
        /// create a new file when the current file create time was than the max
        /// </summary>
        private void NewOrDefualtFileByHour()
        {
            DateTime fileTime = _fileWriter.GetDefualtFileCreateTime();
            TimeSpan diffSpan = fileTime.Subtract(DateTime.Now).Duration();
            double hour = diffSpan.Hours;
            if (hour > _maxFileHour)
            {
                _fileWriter.FilePath = _fileWriter.CreateNewFilePath();
            }
        }

        /// <summary>
        /// create a new file when the current file log cout was than the max
        /// </summary>
        private void NewOrDefualtFileByLogCout()
        {
            if (_currentLogCout > _maxFileLogCout)
            {
                _fileWriter.FilePath = _fileWriter.CreateNewFilePath();
                _currentLogCout = 0;
            }
            else
            {
                _currentLogCout++;
            }
        }

        /// <summary>
        /// set the max file size
        /// </summary>
        /// <param name="pMaxFileSize">the max file size</param>
        public void SetMaxFileSize(double pMaxFileSize)
        {
            _maxFileSize = pMaxFileSize;
        }

        /// <summary>
        /// set the defualt directory path
        /// </summary>
        /// <param name="pDirectoryFilePath">the defualt directory</param>
        public void SetDefualtDirPath(string pDirectoryFilePath)
        {
            _fileWriter.SetDefualtFilePath(pDirectoryFilePath);
        }
    }

    public class FileWriterLogManager:LogManager
    {
        private readonly Logger _loggerImpl;

        public FileWriterLogManager(string directory, string modelName = null)
        {
            _loggerImpl = new FileWriteLogger(directory, modelName);
        }

        protected override Logger GetLoggerImpl(string name)
        {
             return _loggerImpl;
        }
    }
}
