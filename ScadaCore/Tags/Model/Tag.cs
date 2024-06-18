using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.Tags.Model
{

    [DataContract]
    public abstract class Tag
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string IOAddress { get; set; }
        [DataMember]
        public double Value { get; set; }

        public Tag()
        {
        }

        protected Tag(string id, string description, string iOAddress, double value)
        {
            Id = id;
            Description = description;
            IOAddress = iOAddress;
            Value = value;
        }
    }
}