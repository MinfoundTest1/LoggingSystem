﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class TextLogAutoRemover : LogAutoRemover
    {
        private string _directoryPath;//the process log directory
        private static TextLogAutoRemover _textLogAutoRemove;
        /// <summary>
        /// init the class
        /// </summary>
        /// <param name="keepDays">the log file keep days</param>
        /// <param name="directoryPath">the process log directory, accurate to sub folder</param>
        private  TextLogAutoRemover(int keepDays, string directoryPath)
             : base(keepDays)
        {
            CheckDirectory(directoryPath);
        }

        private void CheckDirectory(string directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }
            _directoryPath = directoryPath;
            string modelName = Process.GetCurrentProcess().ProcessName;
            if (!_directoryPath.EndsWith(modelName))
            {
                _directoryPath = Path.Combine(_directoryPath, modelName);
            }
        }

        /// <summary>
        /// remove the log before days
        /// </summary>
        /// <param name="daysBefore">the log file keep days</param>
        protected override void RemoveLogsBefore(int daysBefore)
        {
            if (Directory.Exists(_directoryPath))
            {
                string[] files = Directory.GetFiles(_directoryPath);
                foreach (string filePath in files)
                {
                    FileInfo info = new FileInfo(filePath);
                    DateTime createTimeDay = new DateTime(info.CreationTime.Year, info.CreationTime.Month, info.CreationTime.Day);
                    DateTime currentDays = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan diffSpan = createTimeDay.Subtract(currentDays).Duration();
                    double diffDays = diffSpan.Days;
                    if (diffDays > daysBefore)
                    {
                        File.Delete(filePath);
                    }
                }
            }
           
        }

        /// <summary>
        /// create a TextLogAutoRemover by singleton pattern
        /// </summary>
        /// <param name="keepDays">the log file save days</param>
        /// <param name="directoryPath">the log save total path, or accurate to sub folder</param>
        /// <returns></returns>
        public static TextLogAutoRemover Create(int keepDays, string directoryPath)
        {
            if (_textLogAutoRemove == null)
            {
                _textLogAutoRemove = new TextLogAutoRemover(keepDays, directoryPath);
            }
            return _textLogAutoRemove;
        }
    }
}
