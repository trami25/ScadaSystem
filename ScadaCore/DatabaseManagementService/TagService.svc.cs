using ScadaCore.Tags.Model;
using SimulationDriver;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScadaCore.DatabaseManagementService
{
    public class TagService : ITagService
    {
        private static List<Tag> tags = new List<Tag>();
        private static List<Alarm> alarms = new List<Alarm>();
        private static readonly string AlarmsLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarmsLog1.txt");
        private static readonly Dictionary<string, bool> loggedAlarms = new Dictionary<string, bool>();

        private NamedPipeServerStream pipeServer;

        public TagService()
        {
            Task.Run(() => MonitorTags());
            Task.Run(() => StartNamedPipeServer());
            Console.WriteLine("TagService initialized and monitoring started.");
        }

        public string AddAITag(string tagId, string description, string ioAddress, double value, int scanTime, bool isScanOn, double lowLimit, double highLimit, string unit)
        {
            AnalogInputTag aiTag = new AnalogInputTag(tagId, description, ioAddress, value, scanTime, isScanOn, lowLimit, highLimit, (Unit)Enum.Parse(typeof(Unit), unit), new MainSimulationDriver());
            tags.Add(aiTag);
            string message = $"Analog Input Tag added: {tagId}";
            return message;
        }

        public string AddAOTag(string tagId, string description, string ioAddress, double value, double initialValue, double lowLimit, double highLimit, string unit)
        {
            AnalogOutputTag aoTag = new AnalogOutputTag(tagId, description, ioAddress, value, initialValue, lowLimit, highLimit, (Unit)Enum.Parse(typeof(Unit), unit));
            tags.Add(aoTag);
            return $"Analog Output Tag added: {tagId}";
        }

        public string AddDITag(string tagId, string description, string ioAddress, double value, int scanTime, bool isScanOn)
        {
            DigitalInputTag diTag = new DigitalInputTag(tagId, description, ioAddress, value, scanTime, isScanOn, new MainSimulationDriver());
            tags.Add(diTag);
            return $"Digital Input Tag added: {tagId}";
        }

        public string AddDOTag(string tagId, string description, string ioAddress, double value, double initialValue)
        {
            DigitalOutputTag doTag = new DigitalOutputTag(tagId, description, ioAddress, value, initialValue);
            tags.Add(doTag);
            return $"Digital Output Tag added: {tagId}";
        }

        public string DisableScan(string tagId)
        {
            var inputTag = tags.OfType<InputTag>().FirstOrDefault(t => t.Id == tagId);
            if (inputTag != null)
            {
                inputTag.IsScanOn = false;
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
                return $"Scanning enabled for tag: {tagId}";
            }
            else
            {
                return $"Tag not found for enabling scan: {tagId}";
            }
        }

        public List<Tag> GetAllTags()
        {
            Console.WriteLine($"Retrieving all tags. Total tags: {tags.Count}");
            return tags;
        }

        public List<AnalogInputTag> GetAnalogInputTags()
        {
            var aiTags = tags.OfType<AnalogInputTag>().ToList();
            Console.WriteLine($"Retrieving all analog input tags. Total tags: {aiTags.Count}");
            return aiTags;
        }

        public string RemoveTag(string tagId)
        {
            var tagToRemove = tags.FirstOrDefault(t => t.Id == tagId);
            if (tagToRemove != null)
            {
                tags.Remove(tagToRemove);
                return $"Tag removed: {tagId}";
            }
            else
            {
                return $"Tag not found for removal: {tagId}";
            }
        }

        public string SetOutputValue(string tagId, double value)
        {
            var outputTag = tags.FirstOrDefault(t => t.Id == tagId);
            if (outputTag != null)
            {
                outputTag.Value = value;
                return $"Output value set for tag {tagId}: {value}";
            }
            else
            {
                return $"Tag not found for setting output value: {tagId}";
            }
        }

        public string AddAlarm(string tagName, string type, int priority, double threshold)
        {
            var analogInputTag = tags.OfType<AnalogInputTag>().FirstOrDefault(t => t.Id == tagName);
            if (analogInputTag == null)
            {
                Console.WriteLine($"Failed to add alarm. Tag not found: {tagName}");
                throw new FaultException("Alarm can only be added to an existing analog input tag.");
            }

            var alarm = new Alarm(tagName, type, priority, threshold);
            alarms.Add(alarm);
            return $"Alarm added for tag {tagName}: Type={type}, Priority={priority}, Threshold={threshold}";
        }

        public string RemoveAlarm(string tagName)
        {
            var alarmToRemove = alarms.FirstOrDefault(a => a.TagName == tagName);
            if (alarmToRemove != null)
            {
                alarms.Remove(alarmToRemove);
                loggedAlarms.Remove(tagName); // Remove from logged alarms
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
                                if (!loggedAlarms.ContainsKey(tag.Id) || !loggedAlarms[tag.Id])
                                {
                                    LogAlarm(alarm);
                                    loggedAlarms[tag.Id] = true;
                                    Console.WriteLine($"Alarm triggered for tag {tag.Id}: Type={alarm.Type}, Threshold={alarm.Threshold}");
                                }
                            }
                            else
                            {
                                loggedAlarms[tag.Id] = false; // Reset the alarm state
                            }
                        }
                    }
                }
                await Task.Delay(1000);
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
                    sw.WriteLine(logEntry);
                }

                NotifyClient(logEntry);

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

        private void NotifyClient(string message)
        {
            try
            {
                if (pipeServer != null && pipeServer.IsConnected)
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    pipeServer.Write(messageBytes, 0, messageBytes.Length);
                    pipeServer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to notify client. Exception: {ex.Message}");
            }
        }

        private void StartNamedPipeServer()
        {
            pipeServer = new NamedPipeServerStream("AlarmPipe", PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            Console.WriteLine("Waiting for client connection...");
            pipeServer.BeginWaitForConnection((IAsyncResult result) =>
            {
                try
                {
                    pipeServer.EndWaitForConnection(result);
                    Console.WriteLine("Client connected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect client. Exception: {ex.Message}");
                }
            }, null);
        }
    }
}
