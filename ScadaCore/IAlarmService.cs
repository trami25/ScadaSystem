using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCore
{
    [ServiceContract]
    public interface IAlarmService
    {
        [OperationContract]
        void AddAlarm(string tagName, string type, int priority, double threshold);

        [OperationContract]
        void RemoveAlarm(string tagName);

        [OperationContract]
        List<Alarm> GetActiveAlarms();
    }
}
