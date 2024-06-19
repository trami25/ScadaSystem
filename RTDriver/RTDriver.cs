using DriverApi;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RTDriver
{
    public class RTDriver : IDriver
    {
        private Dictionary<string, (double lowerLimit, double upperLimit)> _limits = new Dictionary<string, (double lowerLimit, double upperLimit)>();
        private Dictionary<string, double> _data = new Dictionary<string, double>();
        private Random _random = new Random();
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RTDriver()
        {
            GenerateKeys();
        }

        private void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                _privateKey = rsa.ExportParameters(true);
                _publicKey = rsa.ExportParameters(false);
            }
        }

        public void ReceiveData(string address, double lowerLimit, double upperLimit)
        {
            _limits[address] = (lowerLimit, upperLimit);
            _data[address] = 0.0;
        }

        public double ReturnValue(string address)
        {
            if (_limits.ContainsKey(address))
            {
                double value = GenerateRandomValue(_limits[address].lowerLimit, _limits[address].upperLimit);
                _data[address] = value;
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

        public double GetDataForAddress(string address)
        {
            return _data.ContainsKey(address) ? _data[address] : 0.0;
        }

        public byte[] SignData(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(_privateKey);
                return rsa.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
            }
        }

        public bool VerifyData(string message, byte[] signature)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(_publicKey);
                return rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), signature);
            }
        }
    }
}
