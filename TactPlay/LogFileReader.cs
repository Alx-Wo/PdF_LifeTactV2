using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TactPlay
{
    class LogFileReader
    {

        private bool keepReading = true;
        private bool readOnlyNewLines = false;
        private string fileName;
        private ILogFileHandler logFileHandler;

        public LogFileReader(string logFile, ILogFileHandler logFileHandler)
        {
            this.fileName = logFile;
            this.logFileHandler = logFileHandler;
        }

        public LogFileReader(string logFile, ILogFileHandler logFileHandler, bool onlyNewLines)
            : this(logFile, logFileHandler)
        {
            this.readOnlyNewLines = onlyNewLines;
        }

        public void Run()
        {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(".");
            fsw.Filter = fileName;
            fsw.EnableRaisingEvents = true;
            fsw.Changed += (s, e) => wh.Set();

            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                string line = "";
                while (line != null && readOnlyNewLines)
                {
                    line = sr.ReadLine();
                }
                while (keepReading)
                {
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        logFileHandler.NewLine(line);
                    }
                    else
                    {
                        wh.WaitOne(50);
                    }
                }
            }

            wh.Close();
        }

        public void StopReading()
        {
            keepReading = false;
        }
    }
}
