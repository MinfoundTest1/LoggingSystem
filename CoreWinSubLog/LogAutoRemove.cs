using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class LogAutoRemove : LogAutoRemover
    {
        private string _directoryPath;

        public LogAutoRemove(int keepDays, string directoryPath)
             : base(keepDays)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }
            _directoryPath = directoryPath;
        }

        protected override void RemoveLogsBefore(int daysBefore)
        {
            string[] files = Directory.GetFiles(_directoryPath);
            foreach (string filePath in files)
            {
                FileInfo info = new FileInfo(filePath);
                DateTime createTime = info.CreationTime;
                TimeSpan diffSpan = createTime.Subtract(DateTime.Now).Duration();
                double diffDays = diffSpan.Days;
                if (diffDays >= daysBefore)
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
