using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
   public  class NewFileWithTimeHelper:IFilePathHelper
    {

        #region Property
        /// <summary>
        /// the log file path
        /// </summary>
        private string _filePath;

        /// <summary>
        /// the log file path
        /// </summary>
        public string _modelName;

        /// <summary>
        /// the log directory path
        /// </summary>
        public string _directoryPath;

        private double _maxFileHour = 2;//Hour

        private string defaultDirectory = @"C:\temp";//the default directory

        #endregion
        public NewFileWithTimeHelper(string pDirectory, string pModelName)
        {
            _modelName = pModelName;
            _directoryPath = pDirectory;
            CheckDirectory();
            CheckFile();
        }

        #region Public Function

        /// <summary>
        /// create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public bool NewFileOrDefualt()
        {
            return NewFileByHour();
        }

        /// <summary>
        /// get file path
        /// </summary>
        /// <returns>file path</returns>
        public string GetFilePath()
        {
            return _filePath;
        }
        #endregion

        #region Private Function

        /// <summary>
        /// create a new file when the current file create time was than the max
        /// </summary>
        private bool NewFileByHour()
        {
            DateTime fileTime = GetDefualtFileCreateTime();
            TimeSpan diffSpan = fileTime.Subtract(DateTime.Now).Duration();
            double hour = diffSpan.Hours;
            if (hour >= _maxFileHour)
            {
                CreateNewFilePath();
                return true;
            }
            return false;
        }

        /// <summary>
        /// check the file
        /// </summary>
        private void CheckFile()
        {
            string[] files = Directory.GetFiles(_directoryPath);
            if (files.Count() == 0)
            {
                CreateNewFilePath();
            }
            else
            {
                _filePath = files.Last();
            }
        }

        /// <summary>
        /// get the file create time
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        private DateTime GetDefualtFileCreateTime()
        {
            FileInfo info = new FileInfo(_filePath);
            return info.CreationTime;
        }

        /// <summary>
        /// check the defualt directory
        /// </summary>
        private void CheckDirectory()
        {
            if (_directoryPath == null)
            {
                _directoryPath = defaultDirectory;
            }

            if (!_directoryPath.EndsWith(_modelName))
            {
                _directoryPath = Path.Combine(_directoryPath, _modelName);
            }

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        /// <summary>
        /// create the log file
        /// </summary>
        /// <returns>the log file path</returns>
        private void CreateNewFilePath()
        {
            string[] files = Directory.GetFiles(_directoryPath);
            int index = files.Count() + 1;
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + index + ".txt";
            _filePath = Path.Combine(_directoryPath, fileName);
            if (!File.Exists(_filePath))
            {
                using (File.Create(_filePath))
                {
                }
            }
        }
        #endregion
    }
}
