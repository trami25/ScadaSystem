using System;
using System.Threading;
using RTDriver;

namespace RTUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter RTU ID:");
            string rtuId = Console.ReadLine();

            Console.WriteLine("Enter RT Driver address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter lower limit:");
            double lowerLimit = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter upper limit:");
            double upperLimit = double.Parse(Console.ReadLine());

            RTDriver.RTDriver driver = new RTDriver.RTDriver();
            driver.ReceiveData(address, lowerLimit, upperLimit);

            while (true)
            {
                double value = driver.ReturnValue(address);
                string message = $"Generated value: {value} for address: {address}";
                byte[] signature = driver.SignData(message);

                Console.WriteLine(message);
                Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");

                // Verifikacija potpisa (simulacija servisa)
                bool isValid = driver.VerifyData(message, signature);
                Console.WriteLine($"Is signature valid? {isValid}");

                Thread.Sleep(500);
            }
        }
    }
}
