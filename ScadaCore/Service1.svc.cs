using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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
            string publicKeyPath = "C:\\Users\\korisnik\\Desktop\\novo\\ScadaSystem\\ScadaCore\\publicKey.pem";
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

        public static RSAParameters LoadPublicKeyFromPem(string publicKeyPath)
        {
            try
            {
                var pem = File.ReadAllText(publicKeyPath);

                // Uklonite BEGIN i END oznake i praznine
                var base64Content = pem
                    .Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Trim();

                // Provera minimalne dužine za očekivani javni ključ
                if (base64Content.Length < 50)
                {
                    throw new InvalidOperationException("Invalid public key length");
                }

                // Dekodiranje Base64 stringa
                byte[] publicKeyBytes;
                try
                {
                    publicKeyBytes = Convert.FromBase64String(base64Content);
                }
                catch (FormatException ex)
                {
                    throw new FormatException("Invalid Base64 string format", ex);
                }

                // Provera da li su dekodirani bajtovi neprazni
                if (publicKeyBytes.Length == 0)
                {
                    throw new InvalidOperationException("Invalid public key content");
                }

                using (var ms = new MemoryStream(publicKeyBytes))
                {
                    using (var reader = new BinaryReader(ms))
                    {
                        // Ostatak vaše implementacije...
                        byte byte1 = reader.ReadByte();
                        // ...
                        // Ostatak koda za učitavanje RSA parametara
                    }
                }

                // Vraćanje RSA parametara ako je sve prošlo bez greške
                return new RSAParameters
                {
                    // Postavite RSA parametre ovde
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading public key: {ex.Message}");
                throw;
            }
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
