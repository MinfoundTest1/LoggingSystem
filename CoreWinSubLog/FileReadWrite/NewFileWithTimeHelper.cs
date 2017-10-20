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
        {
            ModelName = Process.GetCurrentProcess().ProcessName;
            DirectoryPath = pDirectory;
            CheckDirectory();
            CheckFile();
        }


        /// <summary>
        /// create new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        public override bool CreateNewOrDefualt()
        {
            FileInfo info = new FileInfo(FilePath);
            DateTime createTimeDay = new DateTime(info.CreationTime.Year, info.CreationTime.Month, info.CreationTime.Day);
            DateTime currentDays = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan diffSpan = createTimeDay.Subtract(currentDays).Duration();
            double day = diffSpan.Days;
            if (day >= _maxFileDay)
            {
                CreateNewFilePath();
                return true;
            }
            return false;
        }
    }
}
