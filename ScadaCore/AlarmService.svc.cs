using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace ScadaCore
{
    public class AlarmService : IAlarmService
    {
        private static List<Alarm> alarms = new List<Alarm>();
        private static readonly string AlarmsLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarmsLog.txt");

        public void AddAlarm(string tagName, string type, int priority, double threshold)
        {
            var alarm = new Alarm(tagName, type, priority, threshold);
            alarms.Add(alarm);
            LogAlarm(alarm);
            //TODO: store alarm in the database
        }

        public void RemoveAlarm(string tagName)
        {
            var alarmToRemove = alarms.FirstOrDefault(a => a.TagName == tagName);
            if (alarmToRemove != null)
            {
                alarms.Remove(alarmToRemove);
                //TODO: Remove alarm from the database
            }
        }

        public List<Alarm> GetActiveAlarms()
        {
            return alarms;
        }

        private void LogAlarm(Alarm alarm)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(AlarmsLogFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter sw = File.AppendText(AlarmsLogFilePath))
                {
                    sw.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated At: {alarm.ActivationTime}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FaultException<ExceptionDetail>(new ExceptionDetail(ex), new FaultReason("Access to the log file path is denied."));
            }
            catch (Exception ex)
            {
                throw new FaultException<ExceptionDetail>(new ExceptionDetail(ex), new FaultReason("An error occurred while logging the alarm."));
            }
        }
    }
}
