using DriverApi;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        public InputTag()
        {
        }

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

                // TODO: Add only tag values to db
                //using (TagContext context = new TagContext())
                //{
                //    context.Tags.AddOrUpdate((Tag)this);
                //}

                Thread.Sleep(ScanTime);
            }
        }
    }
}