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
            RTDriverServiceClient client = new RTDriverServiceClient();

            Console.Write("Enter address (example: 127.0.0.1): ");
            string address = Console.ReadLine();
            Console.Write("Enter RTU ID: ");
            string rtuId = Console.ReadLine();
            Console.Write("Enter lower limit: ");
            double lowerLimit = double.Parse(Console.ReadLine());
            Console.Write("Enter upper limit: ");
            double upperLimit = double.Parse(Console.ReadLine());

            await client.AddAddressAsync(address, lowerLimit, upperLimit);

            await Task.Run(async () =>
            {
                while (true)
                {
                    double value = await client.ReturnValueAsync(address);
                    Console.WriteLine($"Generated value for {address}: {value}");
                    Thread.Sleep(10000);
                }
            });
        }
    }
}
