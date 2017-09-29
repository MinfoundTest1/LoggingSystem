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
            private set { filePath = value;}
        }

        /// <summary>
        /// the log file path
        /// </summary>
        private string modelName;
        public string ModelName
        {
            get { return modelName; }
            private set { modelName = value; }
        }


        /// <summary>
        /// the log file path
        /// </summary>
        private string directoryPath;
        public string DirectoryPath
        {
            get { return directoryPath; }
            private set { DirectoryPath = value; }
        }

        #endregion

        #region Public Function
        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="pDirectoryPath">the log directory path</param>
        /// <param name="pProcessName">the log create source process</param>
        public TextFileReadWrite(string pDirectoryPath,string pProcessName)
        {
            DirectoryPath = pDirectoryPath;
            ModelName = pProcessName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">the log create time</param>
        /// <param name="level">the log level</param>
        /// <param name="message">the log message info</param>
        /// <param name="args">parames args</param>
        public void Write(LogRecord recoder, params object[] args)
        {
            if (FilePath == null)
                return;
            _readAndWriterLock.EnterWriteLock();
            try
            {
                string log = string.Format("{0} {1} {2} {3}", recoder.ModuleName,recoder.DateTime, recoder.Level, recoder.Message);
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

        /// <summary>
        /// create the log file
        /// </summary>
        /// <returns>the log file path</returns>
        public string CreateFilePath()
        {
            if (DirectoryPath != null)
            {
                if (!DirectoryPath.EndsWith(ModelName))
                {
                    DirectoryPath = Path.Combine(DirectoryPath, ModelName);
                }
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
            }
            string[] files = Directory.GetFiles(directoryPath);
            int index = files.Count() + 1;
            string fileName = DateTime.Now.ToString("YYMMDD"+".txt");
            FilePath = Path.Combine(directoryPath,fileName);
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }

            return FilePath;
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
