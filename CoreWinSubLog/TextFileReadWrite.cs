using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        //private Queue<string> _logFromTextQueue = new Queue<string>();
        /// <summary>
        /// the process model name
        /// </summary>
        private string processName;
        public  string ProcessName
        {
            get { return processName; }
            set { processName = value; }
        }

        /// <summary>
        /// the log file path
        /// </summary>
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value;}
        }

        #endregion
        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="filePath">the log file path</param>
        /// <param name="modelName">the log orgin </param>
        public TextFileReadWrite(string filePath,string modelName)
        {
            FilePath = filePath;
            processName = modelName;
            if (filePath != null)
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
            }
        }

        /// <summary>
        /// the anacy way to write log
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pLogLevel"></param>
        /// <param name="pLogtime"></param>
        public void Write(string pMessage,LogLevel pLogLevel,DateTime pLogtime)
        {
            if (FilePath == null)
                return;
            try
            {
                _readAndWriterLock.EnterWriteLock();
                string log = string.Format("{0} {1} {2} {3}", ProcessName, pLogtime, pLogLevel, pMessage);
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine(log);
                }
            }
            finally
            {
                _readAndWriterLock.ExitWriteLock();
            }
           
        }

        private void WriteLine(string pMessage)
        {
            if (FilePath == null)
                return;
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(pMessage);
            }
        }

        /// <summary>
        /// read a line from log file
        /// </summary>
        /// <param name="pIsDeletLine"></param>
        /// <returns></returns>
        public string ReadLine()
        {
            if (FilePath == null)
                return string.Empty;
            string message = string.Empty;
            string allLinesText = string.Empty;
            _readAndWriterLock.EnterReadLock();
            using (StreamReader read = new StreamReader(FilePath))
            {
                message = read.ReadLine();
            }
            _readAndWriterLock.ExitReadLock();
            return message;
        }

        /// <summary>
        /// delete the first read line 
        /// </summary>
        public void DeteReadLine()
        {
            if (FilePath == null)
            {
                return;
            }
            try
            {
                _readAndWriterLock.EnterUpgradeableReadLock();
                List<string> allTexts = File.ReadAllLines(FilePath).ToList();
                try
                {
                    _readAndWriterLock.EnterWriteLock();
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
    }
}
