using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public abstract class Tag
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }
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