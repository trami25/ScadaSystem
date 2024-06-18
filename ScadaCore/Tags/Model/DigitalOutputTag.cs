using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.Tags.Model
{
    [DataContract]
    public class DigitalOutputTag : Tag
    {
        [DataMember]
        public double InitialValue { get; }

        public DigitalOutputTag()
        {
        }

        public DigitalOutputTag(string id, string description, string ioAddress, double value, double initialValue) : base(id, description, ioAddress, value)
        {
            InitialValue = initialValue;
        }
    }
}