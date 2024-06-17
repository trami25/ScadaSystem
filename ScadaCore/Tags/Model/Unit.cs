using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.Tags.Model
{
    [DataContract]
    public enum Unit
    {
        [EnumMember]
        Kg,
        [EnumMember]
        Ms,
        [EnumMember]
        C,
        [EnumMember]
        F
    }
}