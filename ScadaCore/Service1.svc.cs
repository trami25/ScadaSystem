using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ScadaCore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1, IRTDriverService
    {

        private RSAParameters publicKey;

        public Service1()
        {
            //string publicKeyPath = "C:\\Users\\korisnik\\Desktop\\novo\\ScadaSystem\\ScadaCore\\publickey1.pem";
            //publicKey = LoadPublicKeyFromPem(publicKeyPath);
        }

        public string ReceiveData(string address, double value)  //, byte[] signedMessage
        {
            string message = $"{address}:{value}";
            //byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            //if (VerifyMessage(messageBytes, signedMessage, publicKey))
            //{
            Console.WriteLine($"Received and verified value {value} at address {address}");
            return message;
            //}
            //else
            //{
            //Console.WriteLine($"Verification failed for value {value} at address {address}");
            //}
        }

        public static bool VerifyMessage(byte[] message, byte[] signedMessage, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.VerifyData(message, new SHA256CryptoServiceProvider(), signedMessage);
            }
        }

        public static RSAParameters LoadPublicKeyFromPem(string publicKeyPath)
        {
            // Read the entire PEM file into a string
            string pem = File.ReadAllText(publicKeyPath);

            // Remove the header, footer, and whitespace
            pem = pem.Replace("-----BEGIN PUBLIC KEY-----", "")
                     .Replace("-----END PUBLIC KEY-----", "")
                     .Replace("\n", "")
                     .Replace("\r", "")
                     .Trim();

            // Convert the Base64 string to a byte array
            byte[] publicKeyBytes = Convert.FromBase64String(pem);

            // Use a MemoryStream to read the byte array
            using (var ms = new MemoryStream(publicKeyBytes))
            {
                using (var reader = new BinaryReader(ms))
                {
                    // Check if the first byte is 0x30 (indicating an ASN.1 sequence)
                    byte bt = reader.ReadByte();
                    if (bt == 0x30)
                    {
                        // Read the ASN.1 length
                        ReadLength(reader);

                        // Check for another 0x30 indicating a nested sequence
                        bt = reader.ReadByte();
                        if (bt == 0x30)
                        {
                            // Read the nested ASN.1 length
                            ReadLength(reader);

                            // Skip the object identifier (2 bytes)
                            reader.ReadBytes(2);

                            // Check for the bit string tag (0x03)
                            bt = reader.ReadByte();
                            if (bt == 0x03)
                            {
                                // Read the length of the bit string
                                ReadLength(reader);

                                // Skip the null byte
                                reader.ReadByte();

                                // Check for another 0x30 indicating the RSA key sequence
                                bt = reader.ReadByte();
                                if (bt == 0x30)
                                {
                                    // Read the length of the RSA key sequence
                                    ReadLength(reader);

                                    // Read the modulus and exponent
                                    var modulus = ReadInteger(reader);
                                    var exponent = ReadInteger(reader);

                                    // Return the RSA parameters
                                    return new RSAParameters { Modulus = modulus, Exponent = exponent };
                                }
                            }
                        }
                    }
                }
            }

            // Throw an exception if the PEM file format is invalid
            throw new InvalidOperationException("Invalid PEM file format.");
        }

        private static byte[] ReadInteger(BinaryReader reader)
        {
            // Read the length of the integer
            int length = ReadLength(reader);

            // Read the bytes of the integer
            byte[] bytes = reader.ReadBytes(length);
            return bytes;
        }

        private static int ReadLength(BinaryReader reader)
        {
            // Read the length of the ASN.1 element
            int length = reader.ReadByte();

            if (length == 0x81)
            {
                // Length is in the next byte
                length = reader.ReadByte();
            }
            else if (length == 0x82)
            {
                // Length is in the next two bytes
                length = (reader.ReadByte() << 8) | reader.ReadByte();
            }

            return length;
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
