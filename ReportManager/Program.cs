using ScadaCore;
using System;
using System.Collections.Generic;
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


            string option;
            do
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1. All alarms");
                Console.WriteLine("2. All alarms in specific time period");
                Console.WriteLine("3. All alarms with specific priority");
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

                        while (!DateTime.TryParseExact(startTimeInput, "dd.M.yyyy. HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out startTime) 
                            || startTime > DateTime.Now)
                        {
                            Console.WriteLine("Invalid input or start time is not before the current time. Please enter again:");
                            startTimeInput = Console.ReadLine();
                        }

                        Console.WriteLine($"Start time entered: {startTime}");

                        Console.WriteLine("Enter end time (dd.M.yyyy. HH:mm:ss): ");
                        string endTimeInput = Console.ReadLine();

                        while (!DateTime.TryParseExact(endTimeInput, "dd.M.yyyy. HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out endTime)
                             || endTime < startTime)
                        {
                            Console.WriteLine("Invalid input or end time is not after start time. Please enter again:");
                            endTimeInput = Console.ReadLine();
                        }

                        Console.WriteLine($"End time entered: {endTime}");


                        Console.WriteLine();
                        Console.WriteLine($"Alarms in time period from {startTime} to {endTime}: ");
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

                        foreach(Alarm alarm in alarmsWithTargetPriority)
                        {
                            Console.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Activated at: {alarm.ActivationTime}");
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
