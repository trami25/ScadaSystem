using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScadaCore.DatabaseManagementService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TagService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TagService.svc or TagService.svc.cs at the Solution Explorer and start debugging.
    public class TagService : ITagService
    {

        private static List<Tag> tags = new List<Tag>();

        private static List<Alarm> alarms = new List<Alarm>();

        private static readonly string AlarmsLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarmsLog1.txt");

        public TagService() 
        {
            Task.Run(() => MonitorTags());
        }

        public void AddTag(Tag tag)
        {
           tags.Add(tag);
        }

        public void DisableScan(string tagId)
        {
            var inputTag = tags.OfType<InputTag>().FirstOrDefault(t => t.Id == tagId);
            if (inputTag != null)
            {
                inputTag.IsScanOn = false;
            }
        }

        public void EnableScan(string tagId)
        {
           var inputTag = tags.OfType<InputTag>().FirstOrDefault(t=> t.Id == tagId);
            if(inputTag != null)
            {
                inputTag.IsScanOn = true;
            }
        }

        public List<Tag> GetAllTags()
        {
            return tags;
        }

        public List<AnalogInputTag> GetAnalogInputTags()
        {
            return tags.OfType<AnalogInputTag>().ToList();
        }

        public void RemoveTag(string tagId)
        {
            var tagToRemove = tags.FirstOrDefault(t => t.Id == tagId);
            if(tagToRemove != null)
            {
                tags.Remove(tagToRemove);
            }
        }

        public void SetOutputValue(string tagId, double value)
        {
           var outputTag = tags.FirstOrDefault(t=>t.Id == tagId);
            if (outputTag != null)
            {
                outputTag.Value = value;
            }
        }

        public void AddAlarm(string tagName, string type, int priority, double threshold)
        {
            var analogInputTag = tags.OfType<AnalogInputTag>().FirstOrDefault(t => t.Id == tagName);
            if (analogInputTag == null)
            {
                throw new FaultException("Alarm can only be added to an existing analog input tag.");
            }

            var alarm = new Alarm(tagName, type, priority, threshold);
            alarms.Add(alarm);
        }

        public void RemoveAlarm(string tagName)
        {
            var alarmToRemove = alarms.FirstOrDefault(a => a.TagName == tagName);
            if (alarmToRemove != null)
            {
                alarms.Remove(alarmToRemove);
            }
        }

        public List<Alarm> GetActiveAlarms()
        {
            return alarms;
        }

        private async Task MonitorTags()
        {
            while (true)
            {
                foreach (var tag in tags.OfType<AnalogInputTag>())
                {
                    if (tag.IsScanOn)
                    {
                        foreach (var alarm in alarms.Where(a => a.TagName == tag.Id))
                        {
                            if ((alarm.Type == "High" && tag.Value > alarm.Threshold) ||
                                (alarm.Type == "Low" && tag.Value < alarm.Threshold))
                            {
                                LogAlarm(alarm);
                                // Notify clients about the alarm (e.g., through a callback or polling mechanism)
                            }
                        }
                    }
                }
                await Task.Delay(1000); // Check tags every second
            }
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
                    sw.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated At: {DateTime.Now}");
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
