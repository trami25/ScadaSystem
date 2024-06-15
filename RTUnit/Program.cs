using System;
using System.Threading;
using RTDriver;

namespace RTUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            // RTU configuration
            string address = "127.0.0.1"; // address of RT Driver
            string rtuId = "RTU1";
            double lowerLimit = 0;
            double upperLimit = 100;

            // Create an instance of RT Driver
            RTDriver.RTDriver driver = new RTDriver.RTDriver();

            // Simulation of sending data
            Random random = new Random();
            while (true)
            {
                double value = random.NextDouble() * (upperLimit - lowerLimit) + lowerLimit;
                driver.ReceiveData(address, value);

                // Wait for 1 second before sending the next value
                Thread.Sleep(1000);
            }
        }
    }
}

