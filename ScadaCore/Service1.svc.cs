using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ScadaCore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1, IRTDriverService
    {

        private static Dictionary<string, (double lowerLimit, double upperLimit)> _limits = new Dictionary<string, (double, double)>();
        private static Dictionary<string, List<double>> _data = new Dictionary<string, List<double>>();
        private static Random _random = new Random();

        public void AddAddress(string address, double lowerLimit, double upperLimit)
        {
            _limits[address] = (lowerLimit, upperLimit);
            _data[address] = new List<double>();
        }

        public double ReturnValue(string address)
        {
            if (_limits.ContainsKey(address))
            {
                double value = GenerateRandomValue(_limits[address].lowerLimit, _limits[address].upperLimit);
                _data[address].Add(value);
                return value;
            }
            else
            {
                throw new FaultException($"Address {address} not found.");
            }
        }

        public List<double> GetDataForAddress(string address)
        {
            return _data.ContainsKey(address) ? _data[address] : new List<double>();
        }

        private double GenerateRandomValue(double lowerLimit, double upperLimit)
        {
            return _random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
