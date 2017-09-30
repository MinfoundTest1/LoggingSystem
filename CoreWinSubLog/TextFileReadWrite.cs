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
        private string defaultDirectory= @"C:\temp";//the default directory

        /// <summary>
        /// the log file path
        /// </summary>
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
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
        /// the log directory path
        /// </summary>
        private string directoryPath;
        public string DirectoryPath
        {
            get { return directoryPath; }
            private set { directoryPath = value; }
        }

        #endregion

        #region Public Function
        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="pDirectoryPath">the log directory path</param>
        /// <param name="pProcessName">the log create source process</param>
        public TextFileReadWrite(string pDirectoryPath, string pProcessName)
        {
            ModelName = pProcessName;
            DirectoryPath = pDirectoryPath;
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
        /// create the log file
        /// </summary>
        /// <returns>the log file path</returns>
        public string CreateNewFilePath()
        {
            string[] files = Directory.GetFiles(DirectoryPath);
            int index = files.Count() + 1;
            string fileName = DateTime.Now.ToString("yyyyMMdd_hhmmss_") + index + ".txt";
            FilePath = Path.Combine(directoryPath, fileName);
            if (!File.Exists(FilePath))
            {
                using (File.Create(FilePath))
                {
                }
            }
            return FilePath;
        }

        /// <summary>
        /// get all files in directory
        /// </summary>
        /// <returns>all files path</returns>
        public string[] GetAllFilesFromDir()
        {
            if (DirectoryPath == null)
            {
                return null;
            }
            return Directory.GetFiles(DirectoryPath);
        }

        /// <summary>
        /// get the file size
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns>size(M)</returns>
        public double GetFileSize(string pFilePath)
        {
            FileInfo info = new FileInfo(pFilePath);
            return info.Length / 1024.00 / 1024.00;
        }

        /// <summary>
        /// check the defualt directory
        /// </summary>
        public void CheckDirectory()
        {
            if (DirectoryPath == null)
            {
                DirectoryPath = defaultDirectory;
            }
            if (ModelName != null)
            {
                if (!DirectoryPath.EndsWith(ModelName))
                {
                    DirectoryPath = Path.Combine(DirectoryPath, ModelName);
                }
            }// if modelname==null, do nothing

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        /// <summary>
        /// check the file
        /// </summary>
        public void CheckFile()
        {
            string[] files = GetAllFilesFromDir();
            if (files.Count() == 0)
            {
                FilePath = CreateNewFilePath();
            }
            else
            {
                FilePath = files.Last();
            }

        }

        /// <summary>
        /// get the file create time
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public DateTime GetDefualtFileCreateTime()
        {
            FileInfo info = new FileInfo(FilePath);
            return info.CreationTime;
        }

        /// <summary>
        /// set defulat directory
        /// </summary>
        /// <param name="pDirectoryPath">the defualt dirceory</param>
        public void SetDefualtFilePath(string pDirectoryPath)
        {
            defaultDirectory = pDirectoryPath;
        }

        /// <summary>
        /// get the log count in defualt file 
        /// </summary>
        /// <returns>the log cout</returns>
        public double GetDefualtFileLogCout()
        {
            List<LogRecord> records= ReadAllRecod();
            if (records == null)
                return 0;
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
