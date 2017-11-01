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
        /// the process name
        /// </summary>
        private string _modelName;

        /// <summary>
        /// the file path
        /// </summary>
        protected string _fileName;

        /// <summary>
        /// the log directory path
        /// </summary>
        protected string _fullDirectory;

        /// <summary>
        /// the log directory root
        /// </summary>
        private string _totalDirectory = @"C:\Temp\Log";

        /// <summary>
        /// to create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public abstract bool CreateNewOrDefualt(out string pFileName);

        public FilePathHelper(string pDirectory)
        {
            _modelName = Process.GetCurrentProcess().ProcessName;
            if (pDirectory != null)
            {
                _totalDirectory = pDirectory;
            }
            CheckDirectory();
            CheckFile(); 
        }

        /// <summary>
        /// check the directory, if could not find ths directory, then create
        /// </summary>
        protected void CheckDirectory()
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
        /// check if has file in the directory
        /// </summary>
        protected void CheckFile()
        {
            DirectoryInfo dirinfo = new DirectoryInfo(_fullDirectory);
            FileInfo[] arrFi = dirinfo.GetFiles("*.*");
            if (arrFi.Count() != 0)
            {
                _fileName = arrFi.OrderBy(s => s.CreationTime).Last().FullName;
            }
        }

        /// <summary>
        /// create the log file, return the file path 
        /// </summary>
        /// <returns>the log file path</returns>
        protected string CreateNewFile()
        {
            _fileName = _modelName + DateTime.Now.ToString("_yyyyMMdd") + ".txt";
            _fileName = Path.Combine(_fullDirectory, _fileName);
            if (!File.Exists(_fileName))
            {
                using (File.Create(_fileName))
                {
                }
            }
            return _fileName;
        }

        /// <summary>
        /// get if directory has file, if the return value is true, means the directory is empty
        /// </summary>
        /// <returns>if the directory is empty</returns>
        protected bool IsDirectoryEmpty()
        {
            if (_fileName == null)
                return true;
            return false;
        }
    }
}
