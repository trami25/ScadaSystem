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

        private static readonly TagValueContext tagValueContext = new TagValueContext();

        public InputTag()
        {
        }

        public InputTag(string id, string description, string ioAddress, double value, int scanTime, bool isScanOn, IDriver driver) : base(id, description, ioAddress, value)
        {
            ScanTime = scanTime;
            IsScanOn = isScanOn;
            Driver = driver;
        }

        // TODO: ovde isto objekat za promenu vrednosti
        public void ReadValue()
        {
            TagValue tagValue = new TagValue();
            while (IsScanOn)
            {
                Value = Driver.ReturnValue(IOAddress);

                tagValue.Value = Value;
                tagValue.TagId = Id;
                tagValue.Timestamp = DateTime.Now;

                using (TagValueContext tagValueContext = new TagValueContext())
                {
                    tagValueContext.TagValues.Add(tagValue);
                    tagValueContext.SaveChanges(); // Save changes immediately
                }

                Thread.Sleep(ScanTime);
            }
        }

    }
}