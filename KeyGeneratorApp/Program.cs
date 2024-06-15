using System;

namespace KeyGeneratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyGenerator.GenerateKeys();
            Console.WriteLine("Keys generated successfully.");
        }
    }
}
