using ScadaCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    public class AlarmReports
    {
        private readonly string fileName = "../../../ScadaCore/alarmsLog1.txt";
        private List<Alarm> alarmsList = new List<Alarm>();
        public AlarmReports() {
            alarmsList = GetAllAlarms();
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();
            //Console.WriteLine(fileName);

            foreach (var line in File.ReadLines(fileName))
            {
                //Console.WriteLine(fileName);
                var parts = line.Split(new[] { ", " }, StringSplitOptions.None);
                if (parts.Length == 5)
                {
                    string tagName = parts[0].Split(':')[1].Trim();
                    string type = parts[1].Split(':')[1].Trim();
                    int priority = int.Parse(parts[2].Split(':')[1].Trim());
                    double threshold = double.Parse(parts[3].Split(':')[1].Trim());
                    if (DateTime.TryParseExact(parts[4].Split(new[] { ": " }, StringSplitOptions.None)[1], "dd.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime activationTime))
                    {
                        alarms.Add(new Alarm(tagName, type, priority, threshold) { ActivationTime = activationTime });
                    }
                }
            }
            return alarms;
        }

        public List<Alarm> GetAlarmsInTimePeriod(DateTime startTime, DateTime endTime)
        {
            return alarmsList
                .Where(a => a.ActivationTime >= startTime && a.ActivationTime <= endTime)
                .OrderBy(a => a.Priority)
                .ThenBy(a => a.ActivationTime)
                .ToList();
        }

        public List<Alarm> GetAlarmsWithPriority(int priority)
        {
            return alarmsList
                .Where (a => a.Priority == priority)
                .OrderBy(a => a.ActivationTime)
                .ToList ();
        }
    }
}
