using DriverApi;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class InputTag : Tag
    {
        public int ScanTime { get; set; } // In milliseconds
        public bool IsScanOn { get; set;  }
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

                using (TagContext context = new TagContext())
                {
                    context.Tags.AddOrUpdate((Tag)this);
                }

                Thread.Sleep(ScanTime);
            }
        }
    }
}