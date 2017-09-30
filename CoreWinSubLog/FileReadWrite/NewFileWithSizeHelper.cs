using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class NewFileWithSizeHelper:IFilePathHelper
    {
        #region Property
        /// <summary>
        /// the log file path
        /// </summary>
        private string _filePath;

        /// <summary>
        /// the log file path
        /// </summary>
        public string _modelName { get; set; }

        /// <summary>
        /// the log directory path
        /// </summary>
        public string DirectoryPath { get; private set; }

        private double _maxFileSize = 2;//M

        private string defaultDirectory = @"C:\temp";//the default directory
        #endregion

        public NewFileWithSizeHelper(string pDirectory, string pModelName)
        {
            _modelName = pModelName;
            DirectoryPath = pDirectory;
            CheckDirectory();
            CheckFile();
        }

        #region Public Function

        /// <summary>
        /// set defulat directory
        /// </summary>
        /// <param name="pDirectoryPath">the defualt dirceory</param>
        public void SetDefualtFilePath(string pDirectoryPath)
        {
            defaultDirectory = pDirectoryPath;
        }

        /// <summary>
        /// create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public bool NewFileOrDefualt()
        {
            return NewFileBySize();
        }

        /// <summary>
        /// get file path
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            return _filePath;
        }
        #endregion

        #region Private Function
        /// <summary>
        /// create a new file when the cureent file size was than the max
        /// </summary>
        private bool NewFileBySize()
        {
            double filesize = GetFileSize(_filePath);
            if (filesize > _maxFileSize)
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
            string[] files = Directory.GetFiles(DirectoryPath); 
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

            if (!DirectoryPath.EndsWith(_modelName))
            {
                DirectoryPath = Path.Combine(DirectoryPath, _modelName);
            }

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
            _filePath = Path.Combine(DirectoryPath, fileName);
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
