using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private string _modelName;

        /// <summary>
        /// the log directory path
        /// </summary>
        protected string _fullDirectory;

        private string _totalDirectory = @"C:\Temp\Log";

        /// <summary>
        /// new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public abstract bool CreateNewOrDefualt(ref string pFileName);

        public FilePathHelper(string pDirectory)
        {
            _modelName = Process.GetCurrentProcess().ProcessName;
            _fullDirectory = pDirectory;
            CheckDirectory();
        }

        /// <summary>
        /// check the defualt directory
        /// </summary>
        public void CheckDirectory()
        {
            if (_fullDirectory == null)
            {
                _fullDirectory = _totalDirectory;
            }

            if (!_fullDirectory.EndsWith(_modelName))
            {
                _fullDirectory = Path.Combine(_fullDirectory, _modelName);
            }

            if (!Directory.Exists(_fullDirectory))
            {
                Directory.CreateDirectory(_fullDirectory);
            }
        }

        /// <summary>
        /// create the log file
        /// </summary>
        /// <returns>the log file path</returns>
        public string  CreateNewFile()
        {
            string fileName = _modelName + DateTime.Now.ToString("_yyyyMMdd") + ".txt";
            fileName = Path.Combine(_fullDirectory, fileName);
            if (!File.Exists(fileName))
            {
                using (File.Create(fileName))
                {
                }
            }
            return fileName;
        }
    }
}
