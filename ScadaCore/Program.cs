using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ScadaCore
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(Authentication));
            svc.Open();
            Console.WriteLine("Listening");
            Console.ReadLine();
            svc.Close();
        }
    }
}