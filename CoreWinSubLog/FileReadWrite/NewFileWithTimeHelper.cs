using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class NewFileWithTimeHelper : FilePathHelper
    {

        #region Property
        private double _maxFileHour = 2;//Hour
        #endregion

        public NewFileWithTimeHelper(string pDirectory, string pModelName)
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
            DateTime fileTime = info.CreationTime;
            TimeSpan diffSpan = fileTime.Subtract(DateTime.Now).Duration();
            double hour = diffSpan.Hours;
            if (hour >= _maxFileHour)
            {
                CreateNewFilePath();
                return true;
            }
            return false;
        }
    }
}
