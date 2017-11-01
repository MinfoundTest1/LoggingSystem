using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class NewFileWithTimeHelper : FilePathHelper
    {

        #region Property
        private double _maxFileDay = 1;//day
        #endregion

        public NewFileWithTimeHelper(string pDirectory)
            : base(pDirectory)
        {

        }

        /// <summary>
        /// create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public override bool CreateNewOrDefualt(out string pFileName)
        {
            if (!IsDirectoryEmpty())
            {
                // if the directory is not empty
                FileInfo info = new FileInfo(_fileName);
                DateTime createTimeDay = info.CreationTime.Date;
                TimeSpan diffSpan = createTimeDay.Subtract(DateTime.Now.Date).Duration();
                double day = diffSpan.Days;
                if (day >= _maxFileDay)
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
