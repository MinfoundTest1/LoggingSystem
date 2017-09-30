using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public interface IFilePathHelper
    {
        /// <summary>
        /// new file or defualt
        /// </summary>
        /// <returns>if create new file</returns>
        bool NewFileOrDefualt();

        /// <summary>
        /// get file path
        /// </summary>
        /// <returns>file path</returns>
        string GetFilePath();
    }
}
