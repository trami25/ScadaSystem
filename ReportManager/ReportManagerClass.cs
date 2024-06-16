using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    public class ReportManagerClass
    {
        private List<Alarm> alarms;

        public ReportManagerClass()
        {
            alarms = new List<Alarm>();
            LoadAlarmsFromFile("..//..//..//alarmsLog.txt");

        }

        private void LoadAlarmsFromFile(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|').Select(p => p.Trim()).ToArray();
                    if (parts.Length == 6)
                    {
                        int id = int.Parse(parts[0]);
                        string type = parts[1];
                        int priority = int.Parse(parts[2]);
                        double threshold = double.Parse(parts[3], CultureInfo.InvariantCulture);
                        string measurementName = parts[4];
                        DateTime timestamp = DateTime.ParseExact(parts[5], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        alarms.Add(new Alarm(id, type, priority, threshold, measurementName, timestamp));
                    } else
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Error loading alarms from file: {ex.Message}");
            }
        }

        public void DisplayAllAlarms()
        {
            foreach (var alarm in alarms)
            {
                Console.WriteLine($"ID: {alarm.Id}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Measurement unit: {alarm.MeasurementUnit}, Timestamp: {alarm.Timestamp}");
            }
        }

        public List<Alarm> GetAlarmsInTimeRange(DateTime startTime, DateTime endTime)
        {
            return alarms.Where(a => a.Timestamp >= startTime && a.Timestamp <= endTime)
                .OrderBy(a => a.Priority)
                .ThenBy(a => a.Timestamp)
                .ToList();
        }

        public List<Alarm> GetAlarmsByPriority(int priority)
        {
            return alarms.Where(a => a.Priority == priority)
                .OrderBy(a => a.Timestamp)
                .ToList();
        }
    }
}
