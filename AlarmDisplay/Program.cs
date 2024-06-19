
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceReference1;

class Program
{
    static async Task Main(string[] args)
    {
        TagServiceClient client = new TagServiceClient();
        int iS=0;
        while (true)
        {
           
            try
            {
                var invokedAlarms = await client.GetAlarmsAsync();
                foreach (var alarm in invokedAlarms)
                {
                    for (int i = 0; i < alarm.Priority; i++)
                    {
                        Console.WriteLine($"Alarm triggered: Tag={alarm.TagName}, Type={alarm.Type}, Threshold={alarm.Threshold}, Priority={alarm.Priority}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching invoked alarms: {ex.Message}");
            }

            await Task.Delay(5000);
        }
    }
}

