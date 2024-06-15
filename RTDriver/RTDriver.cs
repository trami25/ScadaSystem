using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace RTDriver
{
    public class RTDriver
    {
        private Dictionary<string, List<double>> _data = new Dictionary<string, List<double>>();
        private byte[] publicKey;

        public RTDriver()
        {
            publicKey = File.ReadAllBytes("C:\\Users\\korisnik\\Desktop\\novo\\ScadaSystem\\RTDriver\\publicKey.pem");
        }

        public void ReceiveData(string address, double value, byte[] signedMessage)
        {
            string message = $"{address}:{value}";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            if (VerifyMessage(messageBytes, signedMessage, publicKey))
            {
                if (!_data.ContainsKey(address))
                {
                    _data[address] = new List<double>();
                }
                _data[address].Add(value);
                Console.WriteLine($"Received and verified value {value} at address {address}");
            }
            else
            {
                Console.WriteLine($"Verification failed for value {value} at address {address}");
            }
        }

        public List<double> GetData(string address)
        {
            return _data.ContainsKey(address) ? _data[address] : new List<double>();
        }

        public static bool VerifyMessage(byte[] message, byte[] signedMessage, byte[] publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportRSAPublicKey(publicKey, out _);
                return rsa.VerifyData(message, signedMessage, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}


