using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexAutoFtp
{
    class Log
    {
        private static object sync = new object();

        internal static void WriteStr(string ex)
        {
            try
            {
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                {
                    Directory.CreateDirectory(pathToLog);
                }

                var filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log", AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
                var fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff} {1}]\r\n", DateTime.Now, ex);
                
                lock (sync) 
                { 
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251")); 
                }
            }
            catch
            {
                
            }
        }
    }
}
