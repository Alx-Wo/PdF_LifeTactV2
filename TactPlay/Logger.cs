using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TactPlay
{
    class Logger
    {
        public static void WriteLogLine(string logfileName, string text)
        {
            string[] logLines = Regex.Split(text, "\r\n|\r|\n");
            string result = "";
            foreach (string line in logLines)
            {
                DateTime time = DateTime.Now;
                result += time.ToString("yyyy-MM-dd HH:mm:ss.fff ");
                result += line;
                result += Environment.NewLine;
            }
            string directory = System.IO.Directory.GetCurrentDirectory();
            System.IO.File.AppendAllText(System.IO.Path.Combine(directory, logfileName), result);
        }
    }
}
