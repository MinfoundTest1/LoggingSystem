using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FilePathHelper
    {
        private double _maxFileSize = 2;//M
        private double _maxFileHour = 2;//Hour
        private double _maxFileLogCout = 10000;
        private double _currentLogCout = 0;
        private string defaultDirectory = @"C:\temp";//the default directory

        #region Property
        /// <summary>
        /// the log file path
        /// </summary>
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            private set { filePath = value; }
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

        public FilePathHelper(string pDirectory, string pModelName)
        {
            ModelName = pModelName;
            DirectoryPath = pDirectory;
            CheckDirectory();
            CheckFile();
        }

        #region Public Function
        /// <summary>
        /// set the max file size
        /// </summary>
        /// <param name="pMaxFileSize">the max file size</param>
        public void SetMaxFileSize(double pMaxFileSize)
        {
            _maxFileSize = pMaxFileSize;
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
        /// switch which way to create new file
        /// </summary>
        /// <param name="pNewFileOpations"></param>
        public void NewFileOrDefualt(NewFileOpations pNewFileOpations)
        {
            switch (pNewFileOpations)
            {
                case NewFileOpations.FileSize:
                    NewFileBySize();
                    break;
                case NewFileOpations.FileTime:
                    NewFileByHour();
                    break;
                case NewFileOpations.LogCount:
                    NewFileByLogCout();
                    break;
            }
        }

        #endregion

        #region Private Function
        /// <summary>
        /// create a new file when the cureent file size was than the max
        /// </summary>
        private void NewFileBySize()
        {
            double filesize = GetFileSize(FilePath);
            if (filesize > _maxFileSize)
            {
                CreateNewFilePath();
            }
        }

        /// <summary>
        /// create a new file when the current file create time was than the max
        /// </summary>
        private void NewFileByHour()
        {
            DateTime fileTime = GetDefualtFileCreateTime();
            TimeSpan diffSpan = fileTime.Subtract(DateTime.Now).Duration();
            double hour = diffSpan.Hours;
            if (hour > _maxFileHour)
            {
                CreateNewFilePath();
            }
        }

        /// <summary>
        /// create a new file when the current file log cout was than the max
        /// </summary>
        private void NewFileByLogCout()
        {
            if (_currentLogCout > _maxFileLogCout)
            {
                CreateNewFilePath();
                _currentLogCout = 0;
            }
            else
            {
                _currentLogCout++;
            }
        }

        /// <summary>
        /// get all files in directory
        /// </summary>
        /// <returns>all files path</returns>
        private string[] GetAllFilesFromDir()
        {
            if (DirectoryPath == null)
            {
                return null;
            }
            return Directory.GetFiles(DirectoryPath);
        }

        /// <summary>
        /// check the file
        /// </summary>
        private void CheckFile()
        {
            string[] files = GetAllFilesFromDir();
            if (files.Count() == 0)
            {
                CreateNewFilePath();
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
        private DateTime GetDefualtFileCreateTime()
        {
            FileInfo info = new FileInfo(FilePath);
            return info.CreationTime;
        }

        /// <summary>
        /// get the file size
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns>size(M)</returns>
        private double GetFileSize(string pFilePath)
        {
            FileInfo info = new FileInfo(pFilePath);
            return info.Length / 1024.00 / 1024.00;
        }

        /// <summary>
        /// check the defualt directory
        /// </summary>
        private void CheckDirectory()
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
        /// create the log file
        /// </summary>
        /// <returns>the log file path</returns>
        private void CreateNewFilePath()
        {
            string[] files = Directory.GetFiles(DirectoryPath);
            int index = files.Count() + 1;
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + index + ".txt";
            FilePath = Path.Combine(directoryPath, fileName);
            if (!File.Exists(FilePath))
            {
                using (File.Create(FilePath))
                {
                }
            }
        }
        #endregion
    }
}
