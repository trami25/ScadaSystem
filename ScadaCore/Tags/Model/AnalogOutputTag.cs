using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class AnalogOutputTag : Tag
    {
        public double InitialValue { get; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
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