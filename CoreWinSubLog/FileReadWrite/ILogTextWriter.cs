using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    /// <summary>
    /// Interface representing to write log to text file.
    /// </summary>
    interface ILogTextWriter
    {
        /// <summary>
        /// Write the module information in first line.
        /// </summary>
        /// <param name="moduleName">Module (Process) name</param>
        void WriteModuleName(string moduleName);

        /// <summary>
        /// Write a log record to the end of text file, which is a string line in text file.
        /// </summary>
        /// <param name="logRecord"></param>
        void WriteLogLine(LogRecord logRecord);
    }
}
