using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCore
{
    [ServiceContract]
    public interface ISimpleService
    {
        [OperationContract]
        string GetSomeMessage(string token);
    }
}
