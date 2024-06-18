using ScadaCore.Tags.Model;
using System;
using System.Runtime.Serialization;

namespace ScadaCore
{
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public string TagName { get; set; }

        [DataMember]
        public string Type { get; set; } // "High" or "Low"

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public double Threshold { get; set; }

        [DataMember]
        public DateTime ActivationTime { get; set; }

        public Unit Unit { get; set; }

        public Alarm()
        {
        }

        public Alarm(string tagName, string type, int priority, double threshold)
        {
            TagName = tagName;
            Type = type;
            Priority = priority;
            Threshold = threshold;
            ActivationTime = DateTime.Now;
        }
    }
}
