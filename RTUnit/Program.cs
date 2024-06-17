using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading;
using ServiceReference1;

namespace RTUnit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter address (example: 127.0.0.1):: ");
            string address = Console.ReadLine();
            Console.Write("Enter RTU ID: ");
            string rtuId = Console.ReadLine();
            Console.Write("Enter lower limit: ");
            double lowerLimit = double.Parse(Console.ReadLine());
            Console.Write("Enter upper limit: ");
            double upperLimit = double.Parse(Console.ReadLine());

          
            using (RTDriverServiceClient driver = new RTDriverServiceClient())
            {
                //byte[] privateKeyBytes = File.ReadAllBytes("privatekey1.pem");
                //RSAParameters privateKeyParams = LoadPrivateKey(privateKeyBytes);

                Random random = new Random();
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        double value = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                        string message = $"{address}:{value}";

                        //byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                        //byte[] signedMessage = SignMessage(messageBytes, privateKeyParams);

                        string mess = await driver.ReceiveDataAsync(address, value);
                        Console.WriteLine($"Received and verified value at address {mess}");
                        // await driver.ReceiveDataAsync(address, value, signedMessage);

                        Thread.Sleep(10000);
                    }
                });

            }
        }

        public static byte[] SignMessage(byte[] message, RSAParameters privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(message, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public static RSAParameters LoadPrivateKey(byte[] privateKeyBytes)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                return rsa.ExportParameters(true);
            }
        }


    }
}