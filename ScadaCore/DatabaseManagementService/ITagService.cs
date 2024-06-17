using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore.DatabaseManagementService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITagService" in both code and config file together.
    [ServiceContract]
    public interface ITagService
    { 
        [OperationContract]
        void AddTag(Tag tag);

        [OperationContract]
        void RemoveTag(string tagId);

        [OperationContract]
        void EnableScan(string tagId);

        [OperationContract]
        void DisableScan(string tagId);

        [OperationContract]
        void SetOutputValue (string tagId, double value);

        [OperationContract]
        List<Tag> GetAllTags();

        [OperationContract]
        List<AnalogInputTag> GetAnalogInputTags();
    }
}
