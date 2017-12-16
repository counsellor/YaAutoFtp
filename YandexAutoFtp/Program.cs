using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using System.Text.RegularExpressions;

namespace YandexAutoFtp
{
    class Program
    {
        static void Main(string[] args)
        {
            var remoteDirectory = "/";
            var remoteArchDirectory = "/Arch/";
            var remoteErrorDirectory = "/Errors/";

            try
            {
                using (var ftp = new FtpClient())
                {
                    ftp.Connect();

                    var files = ftp.GetListing(remoteDirectory);
                    var dict = new Dictionary<string, object>();

                    foreach (var item in files)
                    {
                        if (item.Type == FtpFileSystemObjectType.File)
                        {
                            dict.Add(item.FullName, item);
                        }
                    }

                    var regex = new Regex(@"/\w+_\w+_\d+_\d+.csv$");

                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                throw;
            }
            Console.ReadKey();
        }
    }
}
