using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class NewFileWithSizeHelper:FilePathHelper
    {
        #region Property
        /// <summary>
        /// the log file path
        /// </summary>
        private double _maxFileSize = 2;//M
        #endregion

        public NewFileWithSizeHelper(string pDirectory, string pModelName)
        {
            ModelName = pModelName;
            DirectoryPath = pDirectory;
            CheckDirectory();
            CheckFile();
        }

        /// <summary>
        /// create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public override bool NewFileOrDefualt()
        {
            FileInfo info = new FileInfo(FilePath);
            double filesize = info.Length / 1024.00 / 1024.00;
            if (filesize > _maxFileSize)
            {
                CreateNewFilePath();
                return true;
            }
            return false;
        }
    }
}
