using ScadaCore;
using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    public class Program
    {
        static void Main(string[] args)
        {

            AlarmReports alarmReports = new AlarmReports();
            TagReports tagReports = new TagReports();


            string option;
            do
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1. All alarms");
                Console.WriteLine("2. All alarms in specific time period");
                Console.WriteLine("3. All alarms with specific priority");
                Console.WriteLine("4. All tag values");
                Console.WriteLine("5. All tag values in specific time period");
                Console.WriteLine("6. Latest AI tags values");
                Console.WriteLine("7. Latest DI tags values");
                Console.WriteLine("8. Latest value of all AI tags");
                Console.WriteLine("9. Latest value of all DI tags");
                Console.WriteLine("10. All values for Tag ID");
                Console.WriteLine("X. Exit");
                option = Console.ReadLine().ToUpper();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("All alarms: ");
                        List<Alarm> alarms = alarmReports.GetAllAlarms();

                        foreach (Alarm alarm in alarms)
                        {
                            Console.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated at: {alarm.ActivationTime}");
                        }
                        break;

                    case "2":
                        DateTime startTime;
                        DateTime endTime;

                        Console.WriteLine("Enter start time (dd.M.yyyy. HH:mm:ss): ");
                        string startTimeInput = Console.ReadLine();

                        while (!DateTime.TryParseExact(startTimeInput, "dd.M.yyyy. HH:mm:ss", null, DateTimeStyles.None, out startTime) 
                            || startTime > DateTime.Now)
                        {
                            Console.WriteLine("Invalid input or start time is not before the current time. Please enter again:");
                            startTimeInput = Console.ReadLine();
                        }

                        Console.WriteLine($"Start time entered: {startTime}");

                        Console.WriteLine("Enter end time (dd.M.yyyy. HH:mm:ss): ");
                        string endTimeInput = Console.ReadLine();

                        while (!DateTime.TryParseExact(endTimeInput, "dd.M.yyyy. HH:mm:ss", null, DateTimeStyles.None, out endTime)
                             || endTime < startTime)
                        {
                            Console.WriteLine("Invalid input or end time is not after start time. Please enter again:");
                            endTimeInput = Console.ReadLine();
                        }

                        Console.WriteLine($"End time entered: {endTime}");


                        Console.WriteLine();
                        Console.WriteLine($"Alarms in time period from {startTime} to {endTime} (sorted by priority, than by time): ");
                        List<Alarm> alarmsInTimePeriod = alarmReports.GetAlarmsInTimePeriod(startTime, endTime);

                        foreach (Alarm alarm in alarmsInTimePeriod)
                        {
                            Console.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated at: {alarm.ActivationTime}");
                        }

                        break;

                    case "3":
                        Console.WriteLine("Enter target priority: ");
                        int targetPriority;
                        string targetPriorityInput = Console.ReadLine();

                        while (!int.TryParse(targetPriorityInput, out targetPriority)
                            || targetPriority < 0 || targetPriority > 3)
                        {
                            Console.WriteLine("Invalid input. Please enter again: ");
                            targetPriorityInput = Console.ReadLine();
                        }

                        Console.WriteLine($"Entered priority: {targetPriority}");

                        List<Alarm> alarmsWithTargetPriority = alarmReports.GetAlarmsWithPriority(targetPriority);

                        Console.WriteLine($"Alarms with priority {targetPriority} (sorted by timestamp): ");

                        foreach(Alarm alarm in alarmsWithTargetPriority)
                        {
                            Console.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated at: {alarm.ActivationTime}");
                        }
                        break;

                    case "4":
                        List<TagValue> tagValues = tagReports.GetAllTagValues();
                        Console.WriteLine("All tag values: ");

                        foreach(TagValue tagValue in tagValues)
                        {
                            Console.WriteLine($"Id: {tagValue.Id}, Tag name: {tagValue.TagId}, Value: {tagValue.Value}, Timestamp: {tagValue.Timestamp}");
                        }
                        break; 

                    case "5":
                        DateTime startTagTime;
                        DateTime endTagTime;

                        Console.WriteLine("Enter start time (dd/MM/yyyy HH:mm:ss): ");
                        string startTagTimeInput = Console.ReadLine();

                        while (!DateTime.TryParseExact(startTagTimeInput, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out startTagTime) 
                            || startTagTime > DateTime.Now)
                        {
                            Console.WriteLine("Invalid input. Please enter again:");
                            startTagTimeInput = Console.ReadLine();
                        }

                        Console.WriteLine($"Entered start tag time: {startTagTime}");

                        Console.WriteLine("Enter end time (dd/MM/yyyy HH:mm:ss): ");
                        string endTagTimeInput = Console.ReadLine();

                        while (!DateTime.TryParseExact(endTagTimeInput,"dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out endTagTime)
                            || endTagTime < startTagTime)
                        {
                            Console.WriteLine("Invalid input. Please enter again: ");
                            endTagTimeInput = Console.ReadLine();
                        }
                        Console.WriteLine($"Entered end tag time: {endTagTime}");

                        List<TagValue> tagValuesInTimePeriod = tagReports.GetAllTagValuesInTimePeriod(startTagTime, endTagTime);

                        Console.WriteLine($"Tag values in time period from {startTagTime} to {endTagTime} (sorted by timestamp): ");
                        foreach(TagValue tagValue in tagValuesInTimePeriod)
                        {
                            Console.WriteLine($"Id: {tagValue.Id}, Tag name: {tagValue.TagId}, Value: {tagValue.Value}, Timestamp: {tagValue.Timestamp}");
                        }

                        break;

                    case "6":
                        List<TagValue> latestAITagsValues = tagReports.GetLatestAITagValues();
                        Console.WriteLine("Latest AI tags values: ");

                        foreach (TagValue tagValue in latestAITagsValues)
                        {
                            Console.WriteLine($"Id: {tagValue.Id}, Tag name: {tagValue.TagId}, Value: {tagValue.Value}, Timestamp: {tagValue.Timestamp}");
                        }
                        break;

                    case "7":
                        List<TagValue> latestDITagsValues = tagReports.GetLatestDITagsValues();
                        Console.WriteLine("Latest DI tags values: ");

                        foreach(TagValue tagValue in latestDITagsValues)
                        {
                            Console.WriteLine($"Id: {tagValue.Id}, Tag name: {tagValue.TagId}, Value: {tagValue.Value}, Timestamp: {tagValue.Timestamp}");
                        }
                        break;

                    case "8":
                        TagValue latestAITagValue = tagReports.GetLatestValueAmongAllAITags();
                        Console.WriteLine("Latest tag value among all AI tags: ");
                        Console.WriteLine($"Id: {latestAITagValue.Id}, Tag name: {latestAITagValue.TagId}, Value: {latestAITagValue.Value}, Timestamp: {latestAITagValue.Timestamp}");
                        break;

                    case "9":
                        TagValue latestDITagValue = tagReports.GetLatestValueAmongAllDITags();
                        Console.WriteLine("Latest tag value among all DI tags: ");
                        Console.WriteLine($"Id: {latestDITagValue.Id}, Tag name: {latestDITagValue.TagId}, Value: {latestDITagValue.Value}, Timestamp: {latestDITagValue.Timestamp}");
                        break;

                    case "10":
                        Console.WriteLine("Enter Tag ID: ");
                        string tagIdInput = Console.ReadLine();

                        List<TagValue> tagValuesForTagId = tagReports.GetAllValuesForTag(tagIdInput);
                        Console.WriteLine($"All tag values for tag with id {tagIdInput} (sorted by value): ");
                        foreach (TagValue tagValue in tagValuesForTagId)
                        {
                            Console.WriteLine($"Id: {tagValue.Id}, Tag name: {tagValue.TagId}, Value: {tagValue.Value}, Timestamp: {tagValue.Timestamp}");
                        }
                        break;

                    case "X":
                        Console.WriteLine("Exit from program...");
                        
                        break;
                    default:
                        Console.WriteLine("Invalid option selected.");
                        break;
                }
                Console.WriteLine();
            }
            while (option != "X");
            Console.WriteLine("End of program. Press any key for exit...");
            
            

            //Console.ReadKey();
        }
    }
}
