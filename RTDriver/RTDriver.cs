using System;
using System.Collections.Generic;

namespace RTDriver
{
    public class RTDriver
    {
        private Dictionary<string, List<double>> _data = new Dictionary<string, List<double>>();

        public void ReceiveData(string address, double value)
        {
            if (!_data.ContainsKey(address))
            {
                _data[address] = new List<double>();
            }
            _data[address].Add(value);
            Console.WriteLine($"Received value {value} at address {address}");
        }

        public List<double> GetData(string address)
        {
            return _data.ContainsKey(address) ? _data[address] : new List<double>();
        }
    }
}

