using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    public class LogTextReader: ILogTextReader
    {
        private TextFileReadWrite _fileReader;

        public string DirectoryRoot { private set; get; }

        public LogTextReader(string directoryName)
        {
             CheckDirectoryPath(directoryName);
        }

        private void  CheckDirectoryPath(string pDirectory)
        {
            if (pDirectory == null)
            {
                throw new ArgumentNullException(nameof(pDirectory));
            }

            if (!Directory.Exists(pDirectory))
            {
                throw new DirectoryNotFoundException(pDirectory);
            }
            DirectoryRoot = pDirectory;
        }

        public IEnumerable<LogRecord> ReadAllLogRecords()
        {
            List<LogRecord> records = new List<LogRecord>();
            string[] files = Directory.GetFiles(DirectoryRoot);
            foreach (string item in files)
            {
                _fileReader = new TextFileReadWrite(item);
                records.AddRange(_fileReader.ReadAllRecord());
            }
            return records;
        }

        //public IEnumerable<LogRecord> ReadAllLogRecords()
        //{
        //    bool isend = false;
        //    string[] files = Directory.GetFiles(DirectoryRoot);
        //    List<LogRecord> records=new List<LogRecord> ();
        //    foreach (string  filePath in files)
        //    {
        //        TextFileReadWrite fileReader = new TextFileReadWrite(filePath);
        //        do
        //        {
        //            LogRecord record = null;
        //            isend = fileReader.ReadLogRecordLine(ref record);
        //            if (record != null)
        //            {
        //                yield return record;
        //            }
        //        }
        //        while (!isend);
        //        isend = false;
        //    }
        //}


        //public string ReadLine()
        //{
        //    string[] files = Directory.GetFiles(DirectoryRoot);
        //    if (files.Count() == 0)
        //    {
        //        return string.Empty;
        //    }
        //    string filePath = files[_fileIndex];
        //    if (_fileReader.FilePath != filePath)
        //    {
        //        _fileReader = new TextFileReadWrite(filePath);
        //    }
        //    string message = string.Empty;
        //    bool isReadEnd = _fileReader.ReadLine(ref message);
        //    if (isReadEnd)
        //    {
        //        _fileIndex++;
        //        if (_fileIndex >= files.Count())
        //        {
        //            _fileIndex = 0;
        //        }
        //    }
        //    return message;
        //}
        //public IEnumerable<LogRecord> ReadLogWithLimit(int offset, int count)
        //{
        //    LogRecord[] record = { };
        //    while (count > 0)
        //    {
        //        string message = ReadLine();
        //        if (offset <= 0)
        //        {
        //            yield return LogRecord.FromString(message);
        //            count--;
        //        }
        //        else
        //        {
        //            offset--;
        //        }
        //    }
        //}

    }
}
