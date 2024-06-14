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

        public DigitalInputTag(string id, string description, string ioAddress, double scanTime, bool isScanOn) : base(id, description, ioAddress)
        {
            ScanTime = scanTime;
            IsScanOn = isScanOn;
        }
    }
}