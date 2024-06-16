using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ScadaCore.Configuration;
using ScadaCore.Tags.Model;
using System.Collections.Generic;
using System.Linq;
using SimulationDriver;

namespace ScadaCoreTests
{
    [TestClass]
    public class ScadaConfigurationTests
    {
        private static ScadaConfiguration _scadaConfiguration;
        private static List<Tag> _tags;
        private static MainSimulationDriver _simulationDriver;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _simulationDriver = new MainSimulationDriver();
            _scadaConfiguration = new ScadaConfiguration(@"../../scadaTestConfig.xml", _simulationDriver, _simulationDriver);
            _tags = _scadaConfiguration.GetTags().ToList();
        }

        [TestMethod]
        public void ShouldReturnCorrectSizeOfList()
        {
            Assert.AreEqual(4, _tags.Count);
        }

        [TestMethod]
        public void TagShouldHaveCorrectAttributes()
        {
            var tag = (AnalogOutputTag)_tags.Where(t => t.Id == "ao1").First();

            Assert.IsNotNull(tag);
            Assert.AreEqual("deabc", tag.IOAddress);
            Assert.AreEqual(15.4, tag.InitialValue);
            Assert.AreEqual(Unit.Kg, tag.Unit);
            Assert.AreEqual(111.11, tag.Value);
        }
    }
}
