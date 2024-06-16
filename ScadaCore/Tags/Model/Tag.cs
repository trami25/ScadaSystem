using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public abstract class Tag
    {
        public string Id { get; }
        public string Description { get; }
        public string IOAddress { get; }
        public double Value { get; set; }

        protected Tag(string id, string description, string iOAddress, double value)
        {
            Id = id;
            Description = description;
            IOAddress = iOAddress;
            Value = value;
        }
    }
}