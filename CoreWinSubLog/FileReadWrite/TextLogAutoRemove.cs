﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class TextLogAutoRemove : LogAutoRemover
    {
        private string _directoryPath;//the process log directory

        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="keepDays">the log file keep days</param>
        /// <param name="directoryPath">the process log directory, accurate to sub folder</param>
        public TextLogAutoRemove(int keepDays, string directoryPath)
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

        /// <summary>
        /// remove the log before days
        /// </summary>
        /// <param name="daysBefore">the log file keep days</param>
        protected override void RemoveLogsBefore(int daysBefore)
        {
            string[] files = Directory.GetFiles(_directoryPath);
            foreach (string filePath in files)
            {
                FileInfo info = new FileInfo(filePath);
                DateTime createTimeDay = new DateTime(info.CreationTime.Year, info.CreationTime.Month, info.CreationTime.Day);
                DateTime currentDays = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan diffSpan = createTimeDay.Subtract(currentDays).Duration();
                double diffDays = diffSpan.Days;
                if (diffDays >= daysBefore)
                {
                    File.Delete(filePath);
                }
            }
        }

    }
}