using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog.FileReadWrite
{
    /// <summary>
    /// Interface representing to read the log records from the text file. 
    /// </summary>
    interface ILogTextReader
    {
        /// <summary>
        /// Get all the log records from the text file.
        /// </summary>
        /// <returns></returns>
        IEnumerable<LogRecord> ReadAllLogRecords();
    }
}
