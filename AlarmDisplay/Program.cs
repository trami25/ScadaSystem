using ServiceReference1;
using System;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AlarmServiceClient alarmProxy = new AlarmServiceClient();

            while (true)
            {
                var alarms = await alarmProxy.GetActiveAlarmsAsync();
                foreach (var alarm in alarms)
                {
                    for (int i = 0; i < alarm.Priority; i++)
                    {
                        Console.WriteLine($"Alarm: {alarm.TagName}, Type: {alarm.Type}, Priority: {alarm.Priority}, Activated At: {alarm.ActivationTime}");
                    }
                }

                await Task.Delay(10000);
            }
        }
    }
}
