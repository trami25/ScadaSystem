using ReportManager;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        ReportManagerClass reportManager = new ReportManagerClass();
        Console.WriteLine("All alarms: \n");
        reportManager.DisplayAllAlarms();

        Console.WriteLine();
        Console.WriteLine(new string('=', 100));
        Console.WriteLine();

        DateTime startTime = DateTime.MinValue;
        DateTime endTime = DateTime.MinValue;
        bool validInput = false;

        
        while (!validInput)
        {
            Console.WriteLine("Enter start time (yyyy-MM-dd HH:mm:ss): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
            {
                Console.WriteLine("Invalid format. Please enter start time in the format yyyy-MM-dd HH:mm:ss.");
                continue;
            }

            if (startTime > DateTime.Now)
            {
                Console.WriteLine("Start time must be before current time or current time.");
                continue;
            }

            validInput = true;
        }

        validInput = false;

        while (!validInput)
        {
            Console.WriteLine("Enter end time (yyyy-MM-dd HH:mm:ss): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
            {
                Console.WriteLine("Invalid format. Please enter end time in the format yyyy-MM-dd HH:mm:ss.");
                continue;
            }

            if (endTime < startTime)
            {
                Console.WriteLine("End time must be after the start time.");
                continue;
            }

            if (endTime < DateTime.Now)
            {
                Console.WriteLine("End time must be today or after today.");
                continue;
            }

            validInput = true;
        }

        

        List<Alarm> alarmsInDateRange = reportManager.GetAlarmsInTimeRange(startTime, endTime);

        Console.WriteLine("All alarms in specified time range (sorted by priority, then time): \n");
        foreach(var alarm in alarmsInDateRange)
        {
            Console.WriteLine($"ID: {alarm.Id}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Measurement unit: {alarm.MeasurementUnit}, Timestamp: {alarm.Timestamp}");
        }

        Console.WriteLine();
        Console.WriteLine(new string('=', 100));
        Console.WriteLine();

        int targetPriority;

        Console.WriteLine("Enter target priority (1, 2, or 3): ");
        while (!int.TryParse(Console.ReadLine(), out targetPriority) || (targetPriority < 1 || targetPriority > 3))
        {
            Console.WriteLine("Invalid format. Please enter target priority (1, 2, or 3): ");
        }

        Console.WriteLine($"All alarms of priority {targetPriority} (sorted by time): ");

        List<Alarm> alarmsByPriority = reportManager.GetAlarmsByPriority(targetPriority);
        foreach(var alarm in alarmsByPriority)
        {
            Console.WriteLine($"ID: {alarm.Id}, Type: {alarm.Type}, Priority: {alarm.Priority}, Threshold: {alarm.Threshold}, Measurement unit: {alarm.MeasurementUnit}, Timestamp: {alarm.Timestamp}");
        }

        Console.WriteLine();
        Console.WriteLine(new string('=', 100));
        Console.WriteLine();
    }
}
