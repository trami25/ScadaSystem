using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportService" in both code and config file together.
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        List<Alarm> GetAllAlarms();
        [OperationContract]
        List<Alarm> GetAllAlarmsInTimePeriod(DateTime startTime, DateTime endTime);
        [OperationContract]
        List<Alarm> GetAlarmsWithPriority(int priority);
        [OperationContract]
        List<TagValue> GetAllTagValues();
        [OperationContract]
        List<TagValue> GetAllTagValuesInTimePeriod(DateTime startTime, DateTime endTime);
        [OperationContract]
        List<TagValue> GetLatestAITagsValues();
        [OperationContract]
        List<TagValue> GetLatestDITagsValues();
        [OperationContract]
        TagValue GetLatestValueAmongAllAITags();
        [OperationContract]
        TagValue GetLatestValueAmongAllDITags();
        [OperationContract]
        List<TagValue> GetAllValuesForTag(string tagId);
    }

    public interface ICallback
    {
        [OperationContract (IsOneWay = true)]
        void SendAlarmStartEndTime(string startTimeInput, string endTimeInput);
        [OperationContract (IsOneWay = true)]
        void SendTargetPriority(string targetPriority);
        [OperationContract (IsOneWay = true)]
        void SendTagStartEndTime(string startTagTimeInput, string endTimeTagInput);
        [OperationContract(IsOneWay = true)]
        void SendTargetTagId(string tagIdInput);
    }
}
