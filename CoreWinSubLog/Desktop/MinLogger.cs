using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class MinLogger : Logger
    {
        private WcfLogger _wcfLogger;
        private Logger _localLogger;

        public MinLogger(string hostIPAddress, string filePath)
        {

        }

        public override void Log(LogLevel level, string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        private void OutputLog(LogRecord logRecord)
        {

        }
    }
}
