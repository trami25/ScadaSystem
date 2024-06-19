using RTDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

        public void AddUnit(string address, double lowerLimit, double upperLimit)
        {
            _driver.ReceiveData(address, lowerLimit, upperLimit);
        }
    }
}
