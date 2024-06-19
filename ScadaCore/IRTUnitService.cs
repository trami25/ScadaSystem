using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract]
    public interface IRTUnitService
    {
        [OperationContract(IsOneWay = true)]
        void AddUnit(string address, double lowerLimit, double upperLimit);
    }
}
