using RTUnitTemp.RTUnitServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTUnitTemp
{
    internal class Program
    {
        public static byte[] SignData(string message, RSAParameters privateKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
            }
        }

        static void Main(string[] args)
        {
            RSAParameters privateKey;
            RSAParameters publicKey;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                privateKey = rsa.ExportParameters(true);
                publicKey = rsa.ExportParameters(false);
            }

            var random = new Random();

            var client = new RTUnitServiceClient();

            Console.WriteLine("Enter RTU ID:");
            string rtuId = Console.ReadLine();

            Console.WriteLine("Enter RT Driver address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter lower limit:");
            double lowerLimit = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter upper limit:");
            double upperLimit = double.Parse(Console.ReadLine());

            client.AddUnit(new RTUnit
            {
                Address = address,
                LowerLimit = lowerLimit,
                UpperLimit = upperLimit
            });

            while (true)
            {
                double value = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                string message = $"Generated value: {value} for address: {address}";
                byte[] signature = SignData(value.ToString(), privateKey);

                Console.WriteLine(message);
                Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");

                client.WriteValue(address, value, signature, publicKey);

                Thread.Sleep(500);
            }
        }
    }
}
