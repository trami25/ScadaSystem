using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract]
    public interface ITrendingService
    {
        [OperationContract]
        ICollection<TagData> GetTags();
    }

    [DataContract]
    public class TagData
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public double Value { get; set; }
    }
}
