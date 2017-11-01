using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class NewFileWithSizeHelper : FilePathHelper
    {
        #region Property
        /// <summary>
        /// the log file path
        /// </summary>
        private double _maxFileSize = 1;//M
        #endregion

        public NewFileWithSizeHelper(string pDirectory)
            : base(pDirectory)
        {
        }

        /// <summary>
        /// create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public override bool CreateNewOrDefualt(out string pFileName)
        {
            if (IsDirectoryEmpty())
            {
                // if the directory is not empty
                FileInfo info = new FileInfo(_fileName);
                double filesize = info.Length / 1024.00 / 1024.00;
                if (filesize >= _maxFileSize)
                {
                    pFileName = CreateNewFile();
                    return true;
                }
                pFileName = _fileName;
                return false;
            }
            else
            {
                // if the directory is empty
                pFileName = CreateNewFile();
                return true;
            }
        }
    }
}
