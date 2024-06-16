using DriverApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class AnalogInputTag : InputTag
    {
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public Unit Unit { get; }

        public AnalogInputTag(string id, string description, string ioAddress, double value, int scanTime, bool isScanOn, double lowLimit, double highLimit, Unit unit, IDriver driver) : base(id, description, ioAddress, value, scanTime, isScanOn, driver)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Unit = unit;
        }
    }
}