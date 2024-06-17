using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.Tags.Model
{
    [DataContract]
    public abstract class Tag
    {
        [DataMember]
        public string Id { get; }
        [DataMember]
        public string Description { get; }
        [DataMember]
        public string IOAddress { get; }
        [DataMember]
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