using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{

    [ServiceContract]
    public interface IRTDriverService
    {
        [OperationContract]
        void ReceiveData(string address, double value, byte[] signedMessage);
    }

}
