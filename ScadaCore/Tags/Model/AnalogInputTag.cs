using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class AnalogInputTag : Tag
    {
        public double ScanTime { get; set; } // In milliseconds
        public bool IsScanOn { get; set;  }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public Unit Unit { get; }

        public AnalogInputTag(string id, string description, string ioAddress, double value, double scanTime, bool isScanOn, double lowLimit, double highLimit, Unit unit) : base(id, description, ioAddress, value)
        {
            ScanTime = scanTime;
            IsScanOn = isScanOn;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Unit = unit;
        }
    }
}