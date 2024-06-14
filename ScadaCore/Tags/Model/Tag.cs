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

        protected Tag(string id, string description, string iOAddress)
        {
            Id = id;
            Description = description;
            IOAddress = iOAddress;
        }
    }
}