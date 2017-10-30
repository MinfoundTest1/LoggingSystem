using CoreWinSubDataLib;
using CoreWinSubDBLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLogService
{
    public class LogRepository
    {
        public LogRepository()
        {
            GetOrCreateMySqlProxy();
        }

        public void Save(LogRecord record)
        {
            _loggerMySqlProxy.InsertLog(record);
        }

        public void Save(LogRecord[] records)
        {
            _loggerMySqlProxy.InsertLog(records.ToList());
        }

        public LogRecord[] QueryLogWithLimit(int offset, int count)
        {
            return _loggerMySqlProxy.QueryLogWithLimit(offset, count);
        }
      
        LoggerMySqlProxy _loggerMySqlProxy;

        private LoggerMySqlProxy GetOrCreateMySqlProxy()
        {
            if (_loggerMySqlProxy == null)
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                sqlBuilder.DataSource = "127.0.0.1";
                sqlBuilder.InitialCatalog = "logger";
                sqlBuilder.UserID = "root";
                sqlBuilder.Password = "fmi-drooga";

                _loggerMySqlProxy = new LoggerMySqlProxy(sqlBuilder);
            }
            return _loggerMySqlProxy;
        }
    }
}
