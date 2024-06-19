using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract]
    public interface IRTUnitService
    {
        [OperationContract]
        void AddUnit(RTUnit unit);

        [OperationContract(IsOneWay = true)]
        void WriteValue(string address, double value, byte[] signature, RSAParameters publicKey);
    }

    [DataContract]
    public class RTUnit
    {
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public double LowerLimit { get; set; }
        [DataMember]
        public double UpperLimit { get; set; }
    }
}
