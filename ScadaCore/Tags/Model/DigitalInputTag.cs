using DriverApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class DigitalInputTag : InputTag
    {
        public DigitalInputTag()
        {
        }

        public DigitalInputTag(string id, string description, string ioAddress, double value, int scanTime, bool isScanOn, IDriver driver) : base(id, description, ioAddress, value, scanTime, isScanOn, driver)
        {
        }
    }
}