using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverApi;

namespace RTDriver
{
    public class RTDriver : IDriver
    {
        private Dictionary<string, (double lowerLimit, double upperLimit)> _limits = new Dictionary<string, (double lowerLimit, double upperLimit)>();
        private Dictionary<string, List<double>> _data = new Dictionary<string, List<double>>();
        private Random _random = new Random();

        public void ReceiveData(string address, double lowerLimit, double upperLimit)
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
                throw new ArgumentException($"Address {address} not found.");
            }
        }

        private double GenerateRandomValue(double lowerLimit, double upperLimit)
        {
            return _random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
        }

        public List<double> GetDataForAddress(string address)
        {
            return _data.ContainsKey(address) ? _data[address] : new List<double>();
        }
    }
}
