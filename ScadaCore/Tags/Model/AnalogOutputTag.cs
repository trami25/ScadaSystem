using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.Tags.Model
{
    [DataContract]
    public class AnalogOutputTag : Tag
    {
        [DataMember]
        public double InitialValue { get; }
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }
        [DataMember]
        public Unit Unit { get; }

        public AnalogOutputTag(string id, string description, string ioAddress, double value, double initialValue, double lowLimit, double highLimit, Unit unit) : base(id, description, ioAddress, value)
        {
            InitialValue = initialValue;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Unit = unit;
        }
    }
}