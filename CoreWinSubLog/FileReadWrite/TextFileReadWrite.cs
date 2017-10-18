using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CoreWinSubLog
{
    public class TextFileReadWrite : ILogTextWriter
    {
        #region property
        /// <summary>
        /// the read and writer lock
        /// </summary>
        private static ReaderWriterLockSlim _readAndWriterLock = new ReaderWriterLockSlim();
        /// <summary>
        /// the log file path
        /// </summary>
        public string FilePath { get; private set; }

        private int _readLine = 1;
        #endregion

        #region Public Function

        public TextFileReadWrite(string pFilePath)
        {
            if (pFilePath == null)
            {
                throw new ArgumentNullException(nameof(pFilePath));
            }
            FilePath = pFilePath;
        }

        /// <summary>
        /// Write the module information in first line. Do nothing if there is already module name in the file.
        /// If only logs without module name, the log records will be lost after writing module name.
        /// </summary>
        /// <param name="moduleName"></param>
        public void WriteModuleName(string moduleName)
        {
            if (HasModuleName() == false)
            {
                _readAndWriterLock.EnterWriteLock();
                try
                {
                    using (StreamWriter writer = new StreamWriter(FilePath))
                    {
                        writer.BaseStream.Seek(0, SeekOrigin.Begin);
                        writer.WriteLine(moduleName);
                    }
                }
                finally
                {
                    _readAndWriterLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Write a log record to the end of text file, which is a string line in text file.
        /// </summary>
        /// <param name="record">log recod</param>
        public void WriteLogLine(LogRecord record)
        {
            _readAndWriterLock.EnterWriteLock();
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine(record.ToString());
                }
            }
            finally
            {
                _readAndWriterLock.ExitWriteLock();
            }
        }

        private bool HasModuleName()
        {
            bool result = false;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    if (reader.Peek() >= 0)
                    {
                        string firstLine = reader.ReadLine();
                        if (firstLine.Contains(" ") == false)
                        {
                            result = true;
                        }
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            return result;
        }

        /// <summary>
        /// read a line from file
        /// </summary>
        /// <returns>string msg</returns>
        public string ReadLine()
        {
            if (FilePath == null)
                return string.Empty;
            string message = string.Empty;
            int index = 0;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    while (index < _readLine)
                    {
                        message = read.ReadLine();
                        index++;
                    }
                    if (read.Peek() < 0)
                    {
                        _readLine = 0;
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            _readLine++;
            return message;
        }


        public bool ReadLine(ref string message)
        {
            int index = 0;
            bool isReadEnd = false;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    while (index < _readLine)
                    {
                        message = read.ReadLine();
                        index++;
                    }
                    if (read.Peek() <0)
                    {
                        _readLine = 0;
                        isReadEnd = true;
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            _readLine++;
            return isReadEnd;
        }

        /// <summary>
        /// read the all text in file
        /// </summary>
        /// <returns>the text file</returns>
        public string ReadAllText()
        {
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    message = read.ReadToEnd();
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            return message;
        }

        /// <summary>
        /// read a line from file
        /// </summary>
        /// <returns>log record msg</returns>
        public LogRecord ReadLogRecordLine()
        {
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            int index = 0;
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    while (index < _readLine)
                    {
                        message = read.ReadLine();
                        index++;
                    }
                    if (read.Peek() < 0)
                    {
                        _readLine = 0;
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            _readLine++;
            LogRecord recoder = LogRecord.FromString(message);
            return recoder;
        }

        public bool ReadLogRecordLine(ref LogRecord reocrd)
        {
            string message = string.Empty;
            bool isReadEnd = false;
            _readAndWriterLock.EnterReadLock();
            int index = 0;
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    while (index < _readLine)
                    {
                        message = read.ReadLine();
                        index++;
                    }
                    if (read.Peek() < 0)
                    {
                        _readLine = 0;
                        isReadEnd = true;
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            _readLine++;
            reocrd = LogRecord.FromString(message);
            return isReadEnd;
        }

        /// <summary>
        /// read all log in the file
        /// </summary>
        /// <returns>the all recod log</returns>
        public IEnumerable<LogRecord> ReadAllRecord()
        {
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    while (read.Peek() >= 0)
                    {
                        message = read.ReadLine();
                        
                        if (message != null)
                        {
                            yield return LogRecord.FromString(message);
                        }
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// delete first line 
        /// </summary>
        /// <returns>the first record</returns>
        public LogRecord DeleteFirstLine()
        {
            _readAndWriterLock.EnterUpgradeableReadLock();
            LogRecord record = null;
            try
            {
                List<string> allTexts = File.ReadAllLines(FilePath).ToList();
                _readAndWriterLock.EnterWriteLock();
                try
                {
                    if (allTexts.Count > 0)
                    {
                        record = LogRecord.FromString(allTexts[0]);
                        allTexts.RemoveAt(0);
                        File.WriteAllLines(FilePath, allTexts.ToArray());
                    }
                }
                finally
                {
                    _readAndWriterLock.ExitWriteLock();
                }
            }
            finally
            {
                _readAndWriterLock.ExitUpgradeableReadLock();
            }
            return record;
        }

        /// <summary>
        /// get the log count in defualt file 
        /// </summary>
        /// <returns>the log cout</returns>
        public double GetDefualtFileLogCout()
        {
            List<LogRecord> records = ReadAllRecord() as List<LogRecord>;
            return records.Count();
        }

        #endregion

        #region Private Function
        /// <summary>
        /// write the message to file
        /// </summary>
        /// <param name="pMessage"></param>
        private void WriteLine(string pMessage)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(pMessage);
            }
        }
        #endregion
    }
}
