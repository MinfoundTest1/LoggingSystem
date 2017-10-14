using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class FileReadLogger
    {
        private TextFileReadWrite _fileReader;

        public string DirectoryPath { private set; get; }

        private int _fileIndex = 0;

        public FileReadLogger(string directoryName, string modelName = null)
        {
            DirectoryPath = CheckDirectoryPath(directoryName, modelName);
        }

        private string CheckDirectoryPath(string pDirectory, string modelName)
        {
            if (pDirectory == null)
            {
                throw new ArgumentNullException(nameof(pDirectory));
            }

            string moduleName = modelName ?? Process.GetCurrentProcess().ProcessName;

            if (!pDirectory.EndsWith(moduleName))
            {
                pDirectory = Path.Combine(pDirectory, moduleName);
            }

            if (!Directory.Exists(pDirectory))
            {
                throw new DirectoryNotFoundException(pDirectory);
            }

            return pDirectory;
        }

        public string ReadLine()
        {
            string[] files = Directory.GetFiles(DirectoryPath);
            if (files.Count() == 0)
            {
                return string.Empty;
            }
            string filePath = files[_fileIndex];
            if (_fileReader.FilePath != filePath)
            {
                _fileReader = new TextFileReadWrite(filePath);
            }
            string message = string.Empty;
            bool isReadEnd = _fileReader.ReadLine(ref message);
            if (isReadEnd)
            {
                _fileIndex++;
                if (_fileIndex >= files.Count())
                {
                    _fileIndex = 0;
                }
            }
            return message;
        }

        public void ReadAllFileRecord(ref List<LogRecord> records)
        {
            records = records ?? new List<LogRecord>();
            string[] files = Directory.GetFiles(DirectoryPath);
            foreach (string item in files)
            {
                _fileReader = new TextFileReadWrite(item);
                records.AddRange(_fileReader.ReadAllRecord());
            }
        }

        public IEnumerable<LogRecord> ReadLogWithLimit(int offset, int count)
        {
            LogRecord[] record = { };
            while (count > 0)
            {
                string message = ReadLine();
                if (offset <= 0)
                {
                    yield return LogRecord.FromString(message);
                    count--;
                }
                else
                {
                    offset--;
                }
            }
        }

    }
}
