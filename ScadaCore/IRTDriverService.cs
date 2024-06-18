using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCore
{
    [ServiceContract]
    public interface IRTDriverService
    {
        [OperationContract]
        void AddAddress(string address, double lowerLimit, double upperLimit);

        //[OperationContract]
        //double ReturnValue(string address);

        [OperationContract]
        List<double> GetDataForAddress(string address);
    }

    [DataContract]
    public class AddressData
    {
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public double LowerLimit { get; set; }

        [DataMember]
        public double UpperLimit { get; set; }
    }
}
