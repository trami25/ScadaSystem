using DriverApi;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RTDriver
{
    public class RTDriver : IDriver
    {
        public void AddUnit(RTUnit unit)
        {
            using (var context = new RTUnitContext())
            {
                context.Units.AddOrUpdate(unit);
                context.SaveChanges();
            }
        }

        public double ReturnValue(string address)
        {
            RTUnit unit;
            using (var context = new RTUnitContext())
            {
                unit = context.Units.FirstOrDefault(u => u.Address == address);
            }

            if (unit == null)
            {
                throw new ArgumentException($"Address {address} not found.");
            }

            return unit.Value;
        }

        public void WriteValue(string address, double value, byte[] signature, RSAParameters publicKey)
        {
            RTUnit unit;
            using (var context = new RTUnitContext())
            {
                unit = context.Units.FirstOrDefault(u => u.Address == address);
                if (unit == null)
                {
                    throw new ArgumentException($"Address {address} not found.");
                }

                var valid = VerifyData(value.ToString(), signature, publicKey);
                if (!valid)
                {
                    throw new ArgumentException("Signature not valid");
                }
                unit.Value = value;
                context.SaveChanges();
            }
        }

        public bool VerifyData(string message, byte[] signature, RSAParameters publicKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), signature);
            }
        }
    }
}
