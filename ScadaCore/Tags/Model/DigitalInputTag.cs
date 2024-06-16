using DriverApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class DigitalInputTag : Tag
    {
        public double ScanTime { get; set; } // In milliseconds
        public bool IsScanOn { get; set; }
        public IDriver Driver { get; }

        public DigitalInputTag(string id, string description, string ioAddress, double value, double scanTime, bool isScanOn, IDriver driver) : base(id, description, ioAddress, value)
        {
            ScanTime = scanTime;
            IsScanOn = isScanOn;
            Driver = driver;
        }
    }
}