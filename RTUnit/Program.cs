using System;
using System.Threading;
using RTDriver;

namespace RTUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            // Unos konfiguracionih vrednosti
            Console.WriteLine("Enter RTU ID:");
            string rtuId = Console.ReadLine();

            Console.WriteLine("Enter RT Driver address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter lower limit:");
            double lowerLimit = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter upper limit:");
            double upperLimit = double.Parse(Console.ReadLine());

            // Kreiramo instancu RT Drivera
            RTDriver.RTDriver driver = new RTDriver.RTDriver();
            driver.ReceiveData(address, lowerLimit, upperLimit);

            // Simulacija slanja podataka
            Random random = new Random();
            while (true)
            {
                double value = driver.ReturnValue(address);
                Console.WriteLine($"Generated value: {value} for address: {address}");

                // Čekamo 10 sekundi pre nego što pošaljemo sledeću vrednost
                Thread.Sleep(10000);
            }
        }
    }
}
