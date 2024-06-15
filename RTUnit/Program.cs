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
            string rtuId = Console.ReadLine();  //RTU1

            Console.WriteLine("Enter RT Driver address:");
            string address = Console.ReadLine();  //"127.0.0.1"

            Console.WriteLine("Enter lower limit:");
            double lowerLimit = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter upper limit:");
            double upperLimit = double.Parse(Console.ReadLine());

            RTDriver.RTDriver driver = new RTDriver.RTDriver();

            // simulacija slanja podataka
            Random random = new Random();
            while (true)
            {
                double value = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                driver.ReceiveData(address, value);

                // na svakih 10 sekundi saljem podatke
                Thread.Sleep(10000);
            }
        }
    }
}


