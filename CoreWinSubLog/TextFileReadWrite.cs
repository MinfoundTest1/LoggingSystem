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
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        #endregion

        #region Public Function
        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="pDirectoryPath">the log directory path</param>
        /// <param name="pProcessName">the log create source process</param>
        public TextFileReadWrite(string pFilePath)
        {
            FilePath = pFilePath;
        }

        /// <summary>
        /// write the logrecord
        /// </summary>
        /// <param name="recoder">log recod</param>
        public void Write(LogRecord recoder)
        {
            if (FilePath == null)
                return;
            _readAndWriterLock.EnterWriteLock();
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine(recoder.ToString());
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
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
            if (FilePath == null)
                return string.Empty;
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    message = read.ReadLine();
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            return message;
        }

        /// <summary>
        /// read the all text in file
        /// </summary>
        /// <returns>the text file</returns>
        public string ReadAllText()
        {
            if (FilePath == null)
                return string.Empty;
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
        /// <returns>logrecoder msg</returns>
        public LogRecord ReadLogRecodLine()
        {
            if (FilePath == null)
                return null;
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    message = read.ReadLine();
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            LogRecord recoder = LogRecord.FromString(message);
            return recoder;
        }

        /// <summary>
        /// read all log in the file
        /// </summary>
        /// <returns>the all recod log</returns>
        public List<LogRecord> ReadAllRecod()
        {
            List<LogRecord> records = new List<LogRecord>();
            if (FilePath == null)
                return records;
            string message = string.Empty;
            _readAndWriterLock.EnterReadLock();
            try
            {
                using (StreamReader read = new StreamReader(FilePath))
                {
                    while (message != null)
                    {
                        message = read.ReadLine();
                        if (message != null)
                        {
                            records.Add(LogRecord.FromString(message));
                        }
                    }
                }
            }
            finally
            {
                _readAndWriterLock.ExitReadLock();
            }
            return records;
        }

        /// <summary>
        /// delete the first read line 
        /// </summary>
        public void DeleteReadLine()
        {
            if (FilePath == null)
            {
                return;
            }
            _readAndWriterLock.EnterUpgradeableReadLock();
            try
            {
                List<string> allTexts = File.ReadAllLines(FilePath).ToList();
                _readAndWriterLock.EnterWriteLock();
                try
                {
                    if (allTexts.Count > 0)
                    {
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
        }

        /// <summary>
        /// get the log count in defualt file 
        /// </summary>
        /// <returns>the log cout</returns>
        public double GetDefualtFileLogCout()
        {
            List<LogRecord> records = ReadAllRecod();
            if (records == null)
                return 0;
            return records.Count();
        }

        public void SetFilePath(string pFilePath)
        {
            FilePath = pFilePath;
        }
        #endregion

        #region Private Function
        /// <summary>
        /// write the message to file
        /// </summary>
        /// <param name="pMessage"></param>
        private void WriteLine(string pMessage)
        {
            if (FilePath == null)
                return;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(pMessage);
            }
        }
        #endregion
    }
}
