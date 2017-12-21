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
            var reports = new Dictionary<string, object>();

            using (var ftp = new FtpClient())
            {
                try
                {
                    ftp.Connect();

                    var ftpItems = ftp.GetListing(remoteDirectory);
                    var regex = new Regex(@"report_\w+_\d+_\d+.csv$");

                    foreach (var item in ftpItems)
                    {
                        if (item.Type == FtpFileSystemObjectType.File && regex.IsMatch(item.Name))
                        {
                            reports.Add(item.Name, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var mes = ex.Message;
                    Log.WriteStr(mes);
                }

                /*foreach (var item in reports)
                {
                    Console.WriteLine(item.Key);
                }*/

                foreach (var item in reports)
                {
                    var fileName = item.Key.Split(new char[] { '.' }).First();
                    var fileNameParts = fileName.Split(new char[] { '_' });

                    var fileCreateDate = StringToMonthStartDate(fileNameParts[2], fileNameParts[3]);

                    //01.08.17 будут удалены отчеты за dd.01.17. Хранятся 6 месяцев
                    if (fileCreateDate != null && fileCreateDate <= DateTime.Today.AddMonths(-7))
                    {
                        ftp.DeleteFile(String.Format("{0}{1}", remoteDirectory, item.Key));
                        Log.WriteStr(String.Format("УДАЛЕН ФАЙЛ: {0}{1}", remoteDirectory, item.Key));
                    }
                }
                
            }
            //Console.ReadKey();
        }

        static DateTime? StringToMonthStartDate(string _year, string _mm)
        {
            int year;
            int mm;

            if (int.TryParse(_year, out year) && int.TryParse(_mm, out mm))
            {
                return new DateTime(year, mm, 1);
            }
            return null;
        }
    }
}
