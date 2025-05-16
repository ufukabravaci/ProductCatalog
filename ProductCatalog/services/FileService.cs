using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace ProductCatalog.services
{
    public class FileService
    {
        private readonly string _logFilePath = "error_log.txt";
        public void LogError(string message, string methodName, DateTime timeStamp)
        {
            string logMessage = $"{timeStamp} {methodName} - {message}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }

        public List<string> ReadLogs()
        {
            List<string> lines = new();
            if (File.Exists(_logFilePath))
            {
                foreach (var line in File.ReadLines(_logFilePath))
                {
                    lines.Add(line);
                }
            }
            else
            {
                Console.WriteLine("Log dosyası bulunamadı.");
            }
            return lines;
        }


        public List<string> FilterLogs(DateTime? startDate = null, DateTime? endDate = null, string? keyword = null)
        {
            List<string> filteredLogs = new();

            if (!File.Exists(_logFilePath))
            {
                Console.WriteLine("Log dosyası bulunamadı.");
                return filteredLogs;
            }

            foreach (var line in File.ReadLines(_logFilePath))
            {
                var date = line.Split(" ")[0].Trim();
                var time = line.Split(" ")[1].Trim();
                var dateTime = date + " " + time;
                var logDate = DateTime.Parse(dateTime);
                var message = line.Split(" - ")[1].Trim();
                bool isMatch = true;
                if (startDate.HasValue && logDate < startDate.Value) // tarihler varsa ve uygun değilse kontrolü
                    isMatch = false;

                if (endDate.HasValue && logDate > endDate.Value)
                    isMatch = false;
                //keyword varsa ve uygun değilse kontrolü, küçük-büyük harf duyarlılığını kaldırarak
                if (keyword != null && !message.Contains(keyword, StringComparison.OrdinalIgnoreCase)) 
                    isMatch = false;

                if (isMatch) // her şey uygunsa ekle
                    filteredLogs.Add(line);
            }
            return filteredLogs;
        }
    }
}