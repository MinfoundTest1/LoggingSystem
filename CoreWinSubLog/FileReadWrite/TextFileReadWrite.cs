using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CoreWinSubLog
{
    public class TextFileReadWrite
    {
        #region property
        /// <summary>
        /// the read and writer lock
        /// </summary>
        private static ReaderWriterLockSlim _readAndWriterLock = new ReaderWriterLockSlim();
        /// <summary>
        /// the log file path
        /// </summary>
        private string _filePath;

        private int _readLine = 1;
        #endregion

        #region Public Function

        public TextFileReadWrite(string pFilePath)
        {
            if (pFilePath == null)
            {
                throw new ArgumentNullException(nameof(pFilePath));
            }
            _filePath = pFilePath;
        }

        /// <summary>
        /// write the logrecord
        /// </summary>
        /// <param name="recoder">log recod</param>
        public void Write(LogRecord recoder)
        {
            _readAndWriterLock.EnterWriteLock();
            try
            {
                using (StreamWriter writer = new StreamWriter(_filePath, true))
                {
                    writer.WriteLine(recoder.ToString());
                }
            }
            finally
            {
                _readAndWriterLock.ExitWriteLock();
            }

        }

        /// <summary>
        /// read a line from file
        /// </summary>
        /// <returns>string msg</returns>
        public string ReadLine()
        {
            if (_filePath == null)
                return string.Empty;
            string message = string.Empty;
            int index = 0;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(_filePath))
                {
                    while (index < _readLine)
                    {
                        message = read.ReadLine();
                        index++;
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
                using (StreamReader read = new StreamReader(_filePath))
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
        /// <returns>logrecoder msg</returns>
        public LogRecord ReadLogRecordLine()
        {
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            int index = 0;
            try
            {
                using (StreamReader read = new StreamReader(_filePath))
                {
                    while (index < _readLine)
                    {
                        message = read.ReadLine();
                        index++;
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
                using (StreamReader read = new StreamReader(_filePath))
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
                List<string> allTexts = File.ReadAllLines(_filePath).ToList();
                _readAndWriterLock.EnterWriteLock();
                try
                {
                    if (allTexts.Count > 0)
                    {
                        record = LogRecord.FromString(allTexts[0]);
                        allTexts.RemoveAt(0);
                        File.WriteAllLines(_filePath, allTexts.ToArray());
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
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.WriteLine(pMessage);
            }
        }
        #endregion
    }
}
