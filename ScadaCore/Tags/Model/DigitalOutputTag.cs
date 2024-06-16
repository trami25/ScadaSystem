using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class DigitalOutputTag : Tag
    {
        public double InitialValue { get; }

        public DigitalOutputTag(string id, string description, string ioAddress, double value, double initialValue) : base(id, description, ioAddress, value)
        {
            InitialValue = initialValue;
        }
    }
}