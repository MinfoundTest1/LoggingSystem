using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public abstract class FilePathHelper
    {
        /// <summary>
        /// the log file path
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// the log file path
        /// </summary>
        public string ModelName { get; protected set; }

        /// <summary>
        /// the log directory path
        /// </summary>
        public string DirectoryPath { get; protected set; }

        private string _defaultDirectory = @"C:\temp";//the default directory

        /// <summary>
        /// new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public abstract bool CreateNewOrDefualt();

        /// <summary>
        /// check the defualt directory
        /// </summary>
        public void CheckDirectory()
        {
            if (DirectoryPath == null)
            {
                DirectoryPath = _defaultDirectory;
            }

            if (!DirectoryPath.EndsWith(ModelName))
            {
                DirectoryPath = Path.Combine(DirectoryPath, ModelName);
            }

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
            string[] files = Directory.GetFiles(DirectoryPath);
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
        /// create the log file
        /// </summary>
        /// <returns>the log file path</returns>
        public void CreateNewFilePath()
        {
            string[] files = Directory.GetFiles(DirectoryPath);
            int index = files.Count() + 1;
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + index + ".txt";
            FilePath = Path.Combine(DirectoryPath, fileName);
            if (!File.Exists(FilePath))
            {
                using (File.Create(FilePath))
                {
                }
            }
        }
    }
}
