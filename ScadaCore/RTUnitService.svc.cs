using RTDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    public class RTUnitService : IRTUnitService
    {
        private readonly RTDriver.RTDriver _driver;

        public RTUnitService(RTDriver.RTDriver driver)
        {
            _driver = driver;
        }

        public void AddUnit(RTUnit unit)
        {
            _driver.AddUnit(new RTDriver.RTUnit
            {
                Address = unit.Address,
                LowerLimit = unit.LowerLimit,
                UpperLimit = unit.UpperLimit,
                Value = 0.0
            });
        }

        public void WriteValue(string address, double value, byte[] signature, RSAParameters publicKey)
        {
            try
            {
                _driver.WriteValue(address, value, signature, publicKey);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
