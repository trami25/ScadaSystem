using DriverApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.Tags.Model
{
    [DataContract]
    public class AnalogInputTag : InputTag
    {
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }
        [DataMember]
        public Unit Unit { get; }

        public ICollection<Alarm> Alarms { get; set; }

        public AnalogInputTag()
        {
        }

        public AnalogInputTag(string id, string description, string ioAddress, double value, int scanTime, bool isScanOn, double lowLimit, double highLimit, Unit unit, IDriver driver, ICollection<Alarm> alarms) : base(id, description, ioAddress, value, scanTime, isScanOn, driver)
        {
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Unit = unit;
            Alarms = alarms;
        }
    }
}