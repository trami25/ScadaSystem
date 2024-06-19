using ScadaCore.Tags.Model;
using System;
using ScadaCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScadaCore.Tags
{
    public class TagProcessor
    {
        private readonly Dictionary<string, Task> _tagIdToTask = new Dictionary<string, Task>();
        private readonly TagContext _context;
        private static   DatabaseManagementService.TagService _tagService;
        private readonly List<Alarm> invokedAlarms = new List<Alarm>();
        private static readonly string AlarmsLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarmsLog1.txt");

        public void AddTagTask(InputTag tag)
        {
            Task task = new Task(async () => await ReadAndMonitorTag(tag));
            _tagIdToTask[tag.Id] = task;
            task.Start();
        }

        public void DeleteTagTask(string id)
        {
            if (_tagIdToTask.TryGetValue(id, out Task task))
            {
                task.Dispose();
                _tagIdToTask.Remove(id);
            }
        }

        private async Task ReadAndMonitorTag(InputTag tag)
        {
            while (true)
            {
                tag.ReadValue();
                await MonitorTag(tag);
                await Task.Delay(tag.ScanTime);
            }
        }

        private async Task MonitorTag(InputTag tag)
        {
            if (tag is AnalogInputTag analogTag && analogTag.IsScanOn)
            {
                foreach (var alarm in analogTag.Alarms)
                {
                    bool isAlarmConditionMet = (alarm.Type == "High" && analogTag.Value > alarm.Threshold) ||
                                               (alarm.Type == "Low" && analogTag.Value < alarm.Threshold);

                    if (isAlarmConditionMet)
                    {
                        _tagService.GetAlarms().Add(alarm);
                        LogAlarm(alarm, analogTag.Id);
                        invokedAlarms.Add(alarm);
                        Console.WriteLine($"Alarm triggered for tag {analogTag.Id}: Type={alarm.Type}, Threshold={alarm.Threshold}");
                    }
                }
            }
        }

        private void LogAlarm(Alarm alarm, string id)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(AlarmsLogFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string logEntry = $"Tag: {id}, Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated At: {DateTime.Now}";

                using (StreamWriter sw = File.AppendText(AlarmsLogFilePath))
                {
                    for (int i = 0; i < alarm.Priority; i++)
                    {
                        sw.WriteLine(logEntry);
                    }
                }

                Console.WriteLine($"Alarm logged for tag {alarm.TagName}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Failed to log alarm. Access denied to log file path. Exception: {ex.Message}");
                throw new FaultException<ExceptionDetail>(new ExceptionDetail(ex), new FaultReason("Access to the log file path is denied."));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log alarm. Exception: {ex.Message}");
                throw new FaultException<ExceptionDetail>(new ExceptionDetail(ex), new FaultReason("An error occurred while logging the alarm."));
            }
        }
    }
}