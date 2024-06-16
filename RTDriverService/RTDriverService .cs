using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RTDriverService
{
   
    public class RTDriverService : IRTDriverService
    {
        private RSAParameters publicKey;

        public RTDriverService()
        {
            string publicKeyPath = "C:\\Users\\korisnik\\Desktop\\novo\\ScadaSystem\\RTDriverService\\publicKey.pem";
            publicKey = LoadPublicKeyFromPem(publicKeyPath);
        }

        public void ReceiveData(string address, double value, byte[] signedMessage)
        {
            string message = $"{address}:{value}";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            if (VerifyMessage(messageBytes, signedMessage, publicKey))
            {
                Console.WriteLine($"Received and verified value {value} at address {address}");
            }
            else
            {
                Console.WriteLine($"Verification failed for value {value} at address {address}");
            }
        }

        public static bool VerifyMessage(byte[] message, byte[] signedMessage, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.VerifyData(message, new SHA256CryptoServiceProvider(), signedMessage);
            }
        }

        private static RSAParameters LoadPublicKeyFromPem(string publicKeyPath)
        {
            var pem = File.ReadAllText(publicKeyPath);
            var publicKeyBytes = Convert.FromBase64String(pem.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Trim());

            using (var ms = new MemoryStream(publicKeyBytes))
            {
                using (var reader = new BinaryReader(ms))
                {
                    byte byte1 = reader.ReadByte();
                    byte byte2 = reader.ReadByte();
                    if (byte1 == 0x30 && byte2 == 0x81)
                    {
                        reader.ReadByte(); // Skip byte indicating key length
                    }

                    byte1 = reader.ReadByte();
                    if (byte1 == 0x30)
                    {
                        reader.ReadByte(); // Skip byte indicating key length
                    }

                    byte1 = reader.ReadByte();
                    if (byte1 == 0x06)
                    {
                        reader.ReadByte(); // Skip byte indicating algorithm ID length
                        reader.ReadByte(); // Skip algorithm ID
                    }

                    reader.ReadByte(); // Skip bit string identifier
                    reader.ReadByte(); // Skip unused bits

                    if (reader.ReadByte() != 0x03)
                    {
                        throw new InvalidOperationException("Invalid PEM format");
                    }

                    reader.ReadByte(); // Skip null byte

                    if (reader.ReadByte() != 0x30)
                    {
                        throw new InvalidOperationException("Invalid PEM format");
                    }

                    reader.ReadByte(); // Skip null byte

                    return new RSAParameters
                    {
                        Modulus = reader.ReadBytes(256),
                        Exponent = reader.ReadBytes(3)
                    };
                }
            }
        }
    }

}
