using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CoreWinSubLog
{
    public class TextFileReadWrite : TextWriter
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
            private set { filePath = value;}
        }


        /// <summary>
        /// the log file path
        /// </summary>
        private string modelName;
        public string ModelName
        {
            get { return modelName; }
             set { modelName = value; }
        }

        public override Encoding Encoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="filePath">the log file path</param>
        /// <param name="modelName">the process name</param>
        public TextFileReadWrite(string filePath,string modelName)
        {
            FilePath = filePath;
            ModelName = modelName;
            if (filePath != null)
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">the log create time</param>
        /// <param name="level">the log level</param>
        /// <param name="message">the log message info</param>
        /// <param name="args">parames args</param>
        public void Write(DateTime time,LogLevel level, string message, params object[] args)
        {
            if (FilePath == null)
                return;
            _readAndWriterLock.EnterWriteLock();
            try
            {
                string log = string.Format("{0} {1} {2} {3}", ModelName, time, level, message);
                if (args != null)
                {
                    if (args.Count() > 0)
                    {
                        foreach (var item in args)
                        {
                            log += args;
                        }
                    }
                }
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

       /// <summary>
       /// read a line from file
       /// </summary>
       /// <returns></returns>
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
            DeleteReadLine();
            return message;
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
    }
}
