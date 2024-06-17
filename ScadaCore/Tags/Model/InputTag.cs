using DriverApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Web;

namespace ScadaCore.Tags.Model
{
    [DataContract]
    public class InputTag : Tag
    {
        [DataMember]
        public int ScanTime { get; set; } // In milliseconds
        [DataMember]
        public bool IsScanOn { get; set;  }
        [DataMember]
        public IDriver Driver { get; }

        public InputTag(string id, string description, string ioAddress, double value, int scanTime, bool isScanOn, IDriver driver) : base(id, description, ioAddress, value)
        {
            ScanTime = scanTime;
            IsScanOn = isScanOn;
            Driver = driver;
        }

        public void ReadValue()
        {
            while (IsScanOn)
            {
                Value = Driver.ReturnValue(IOAddress);
                Thread.Sleep(ScanTime);
            }
        }
    }
}