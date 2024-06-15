using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using RTDriver;

namespace RTUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Unesite adresu (primer: 127.0.0.1):");
            string address = Console.ReadLine();

            Console.WriteLine("Unesite ID RTU-a:");
            string rtuId = Console.ReadLine();

            Console.WriteLine("Unesite donju granicu:");
            double lowerLimit = double.Parse(Console.ReadLine());

            Console.WriteLine("Unesite gornju granicu:");
            double upperLimit = double.Parse(Console.ReadLine());

            RTDriver.RTDriver driver = new RTDriver.RTDriver();
            byte[] privateKey = File.ReadAllBytes("C:\\Users\\korisnik\\Desktop\\novo\\ScadaSystem\\RTUnit\\privateKey.pem");

            Random random = new Random();
            while (true)
            {
                double value = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                string message = $"{address}:{value}";

                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] signedMessage = SignMessage(messageBytes, privateKey);

                driver.ReceiveData(address, value, signedMessage);

                Thread.Sleep(10000); // slanje poruke na svakih 10 sec
            }
        }

        public static byte[] SignMessage(byte[] message, byte[] privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportRSAPrivateKey(privateKey, out _);
                return rsa.SignData(message, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}




