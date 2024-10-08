﻿using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using SimulationDriver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScadaCore.DatabaseManagementService
{
    public class TagService : ITagService
    {
        private static List<Tag> tags = new List<Tag>();
        private static List<Alarm> alarms = new List<Alarm>();
        private static List<Alarm> invokedAlarms = new List<Alarm>();
        private static readonly string AlarmsLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarmsLog1.txt");
        private static readonly TagValueContext tagValueContext = new TagValueContext();
        private ScadaCore.Tags.TagService tagService1;
     
        private NamedPipeServerStream pipeServer;

        public TagService()
        {
            tagService1 = new Tags.TagService(new TagRepository(new Configuration.ScadaConfiguration(@"../../scadaConfig.xml", new MainSimulationDriver(), new RTDriver.RTDriver())), new TagProcessor());
            tags = tagService1.GetAll().ToList();
/*
            Task.Run(() => MonitorTags());
            Console.WriteLine("TagService initialized and monitoring started.");
*/
        }

        public List<Alarm> GetAlarms()
        {
            var alarmsToReturn = new List<Alarm>(invokedAlarms);
            invokedAlarms.Clear();
            return alarmsToReturn;
        }
        public string AddAITag(string tagId, string description, string ioAddress, double value, int scanTime, bool isScanOn, double lowLimit, double highLimit, string unit, string driver)
        {
            AnalogInputTag aiTag;
            if (driver == "r")
            {
                aiTag = new AnalogInputTag(tagId, description, ioAddress, value, scanTime, isScanOn, lowLimit, highLimit, (Unit)Enum.Parse(typeof(Unit), unit), new RTDriver.RTDriver(), new List<Alarm>());
            }
            else
            {
                 aiTag = new AnalogInputTag(tagId, description, ioAddress, value, scanTime, isScanOn, lowLimit, highLimit, (Unit)Enum.Parse(typeof(Unit), unit), new MainSimulationDriver(), new List<Alarm>());
            }
                tags.Add(aiTag);
                tagService1.Create(aiTag);
            string message = $"Analog Input Tag added: {tagId}";
            return message;
        }

        public string AddAOTag(string tagId, string description, string ioAddress, double value, double initialValue, double lowLimit, double highLimit, string unit)
        {
            AnalogOutputTag aoTag = new AnalogOutputTag(tagId, description, ioAddress, value, initialValue, lowLimit, highLimit, (Unit)Enum.Parse(typeof(Unit), unit));
            tags.Add(aoTag);
            tagService1.Create(aoTag);
            return $"Analog Output Tag added: {tagId}";
        }

        public string AddDITag(string tagId, string description, string ioAddress, double value, int scanTime, bool isScanOn, string driver)
        {
            DigitalInputTag diTag;
            if (driver == "r")
            {
                diTag = new DigitalInputTag(tagId, description, ioAddress, value, scanTime, isScanOn, new RTDriver.RTDriver());
            }
            else
            {
                diTag = new DigitalInputTag(tagId, description, ioAddress, value, scanTime, isScanOn, new MainSimulationDriver());
            }
            tags.Add(diTag);
            tagService1.Create(diTag);
            return $"Digital Input Tag added: {tagId}";
        }

        public string AddDOTag(string tagId, string description, string ioAddress, double value, double initialValue)
        {
            DigitalOutputTag doTag = new DigitalOutputTag(tagId, description, ioAddress, value, initialValue);
            tags.Add(doTag);
            tagService1.Create(doTag);
            return $"Digital Output Tag added: {tagId}";
        }

        public string DisableScan(string tagId)
        {
            var inputTag = tags.OfType<InputTag>().FirstOrDefault(t => t.Id == tagId);
            if (inputTag != null)
            {
                inputTag.IsScanOn = false;
                tagService1.Update(tagId, inputTag);
                return $"Scanning disabled for tag: {tagId}";
            }
            else
            {
                return $"Tag not found for disabling scan: {tagId}";
            }
        }

        public string EnableScan(string tagId)
        {
            var inputTag = tags.OfType<InputTag>().FirstOrDefault(t => t.Id == tagId);
            if (inputTag != null)
            {
                inputTag.IsScanOn = true;
                tagService1.Update(tagId , inputTag);
                return $"Scanning enabled for tag: {tagId}";
            }
            else
            {
                return $"Tag not found for enabling scan: {tagId}";
            }
        }

        public ICollection<TagData> GetAllTags()
        {
            Console.WriteLine($"Retrieving all tags. Total tags: {tags.Count}");
            return tags.Select(t => new TagData { Id = t.Id, Value = t.Value })
                .ToList(); ;
        }

        public ICollection<TagData> GetAnalogInputTags()
        {
            var aiTags = tags.OfType<AnalogInputTag>().ToList();
            Console.WriteLine($"Retrieving all analog input tags. Total tags: {aiTags.Count}");
            return aiTags.Select(t => new TagData { Id = t.Id, Value = t.Value })
                .ToList(); ;
        }
        public ICollection<TagData> GetInputTags()
        {
            var aiTags = tags.OfType<InputTag>().ToList();
            Console.WriteLine($"Retrieving all analog input tags. Total tags: {aiTags.Count}");
            return aiTags.Select(t => new TagData { Id = t.Id, Value = t.Value })
                .ToList(); ;
        }

        public ICollection<TagData> GetOutputTags()
        {
            var aiTags = tags.Except(tags.OfType<InputTag>()).ToList();
            Console.WriteLine($"Retrieving all analog input tags. Total tags: {aiTags.Count}");
            return aiTags.Select(t => new TagData { Id = t.Id, Value = t.Value })
                .ToList(); ;
        }

        public string RemoveTag(string tagId)
        {
            var tagToRemove = tags.FirstOrDefault(t => t.Id == tagId);
            if (tagToRemove != null)
            {
                tags.Remove(tagToRemove);
                tagService1.Delete(tagId);
                return $"Tag removed: {tagId}";
            }
            else
            {
                return $"Tag not found for removal: {tagId}";
            }
        }

        // TODO: objekat koji belezi u bazi kada je tag promenjen (tagId, value, vreme)
        public string SetOutputValue(string tagId, double value)
        {
            var outputTag = tags.FirstOrDefault(t => t.Id == tagId);
            TagValue tagValue = new TagValue();
            if (outputTag != null)
            {
                outputTag.Value = value;
                tagValue.TagId = tagId;
                tagValue.Value = value;
                tagValue.Timestamp = DateTime.Now;
                using (TagValueContext context = new TagValueContext())
                {
                    tagValueContext.TagValues.Add(tagValue);
                    tagValueContext.SaveChanges();
                }
                tagService1.Update(tagId, outputTag);
                return $"Output value set for tag {tagId}: {value}";
                
            }
            else
            {
                return $"Tag not found for setting output value: {tagId}";
            }
        }

        public string AddAlarm(string tagId, string tagName, string type, int priority, double threshold)
        {
            var analogInputTag = tags.OfType<AnalogInputTag>().FirstOrDefault(t => t.Id == tagId);
            if (analogInputTag == null)
            {
                Console.WriteLine($"Failed to add alarm. Tag not found: {tagName}");
                throw new FaultException("Alarm can only be added to an existing analog input tag.");
            }

            var alarm = new Alarm(tagName, type, priority, threshold);
            alarms.Add(alarm);
            tagService1.AddAlarmToAnalogInputTag(tagId, alarm);
            return $"Alarm added for tag {tagName}: Type={type}, Priority={priority}, Threshold={threshold}";
        }

        public string RemoveAlarm(string tagId, string tagName)
        {
            var alarmToRemove = alarms.FirstOrDefault(a => a.TagName == tagId);
            if (alarmToRemove != null)
            {
                alarms.Remove(alarmToRemove);
                tagService1.RemoveAlarmFromAnalogInputTag(tagId, tagName);
           
                return $"Alarm removed for tag: {tagName}";
            }
            else
            {
                return $"Alarm not found for removal: {tagName}";
            }
        }

        public List<Alarm> GetActiveAlarms()
        {
            Console.WriteLine($"Retrieving all active alarms. Total alarms: {alarms.Count}");
            return alarms;
        }

        private async Task MonitorTags()
        {
            int scanTime = 0;
            while (true)
            {
                foreach (var tag in tags.OfType<AnalogInputTag>())
                {
                    if (tag.IsScanOn)
                    {
                        foreach (var alarm in alarms.Where(a => a.TagName == tag.Id))
                        {
                            bool isAlarmConditionMet = (alarm.Type == "High" && tag.Value > alarm.Threshold) ||
                                                       (alarm.Type == "Low" && tag.Value < alarm.Threshold);

                            if (isAlarmConditionMet)
                            {
                                    LogAlarm(alarm);
                                    invokedAlarms.Add(alarm);
                                    Console.WriteLine($"Alarm triggered for tag {tag.Id}: Type={alarm.Type}, Threshold={alarm.Threshold}");
                                
                            }
                        }
                    }
                    scanTime = tag.ScanTime;
                }
                await Task.Delay(scanTime);
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

                string logEntry = $"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated At: {DateTime.Now}";

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
