using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    public class Alarm
    {
        public int Id {  get; set; }
        public string Type { get; set; }
        public int Priority {  get; set; }
        public double Threshold {  get; set; }
        public string MeasurementUnit { get; set; }
        public DateTime Timestamp { get; set; }

        public Alarm()
        {
        }

        public Alarm(int id, string type, int priority, double threshold, string measurementUnit, DateTime timestamp)
        {
            Id = id;
            Type = type;
            Priority = priority;
            Threshold = threshold;
            MeasurementUnit = measurementUnit;
            Timestamp = timestamp;
        }
    }
}
