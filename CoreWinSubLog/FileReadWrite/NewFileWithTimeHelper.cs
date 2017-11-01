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
        public override bool CreateNewOrDefualt(ref string pFileName)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(_fullDirectory);
            FileInfo[] arrFi = dirinfo.GetFiles("*.*");
            if (arrFi.Count() == 0)
            {
                pFileName = CreateNewFile();
                return true;
            }
            else
            {
                string fileName = arrFi.OrderBy(s => s.CreationTime).Last().FullName;
                FileInfo info = new FileInfo(fileName);
                DateTime createTimeDay = info.CreationTime.Date;
                //DateTime currentDays = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan diffSpan = createTimeDay.Subtract(DateTime.Now.Date).Duration();
                double day = diffSpan.Days;
                if (day >= _maxFileDay)
                {
                    pFileName = CreateNewFile();
                    return true;
                }
                pFileName = fileName;
                return false;
            }

        }
    }
}
