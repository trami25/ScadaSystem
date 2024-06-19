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
        string AddAITag(string tagId, string description, string ioAddress, double value, int scanTime, bool isScanOn, double lowLimit, double highLimit, string unit, string driver);

        [OperationContract]
        string AddAOTag(string tagId, string description, string ioAddress, double value, double initialValue, double lowLimit, double highLimit, string unit);

        [OperationContract]
        string AddDITag(string tagId, string description, string ioAddress, double value, int scanTime, bool isScanOn, string driver);

        [OperationContract]
        string AddDOTag(string tagId, string description, string ioAddress, double value, double initialValue);


        [OperationContract]
        string RemoveTag(string tagId);

        [OperationContract]
        string EnableScan(string tagId);

        [OperationContract]
        string DisableScan(string tagId);

        [OperationContract]
        string SetOutputValue (string tagId, double value);

        [OperationContract]
        ICollection<TagData> GetAllTags();

        [OperationContract]
        ICollection<TagData> GetAnalogInputTags();

        [OperationContract]
        ICollection<TagData> GetOutputTags();

        [OperationContract]
        string AddAlarm(string tagName, string type, int priority, double threshold);

        [OperationContract]
        string RemoveAlarm(string tagName);

        [OperationContract]
        ICollection<TagData> GetInputTags();

        [OperationContract]
        List<Alarm> GetAlarms();
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
