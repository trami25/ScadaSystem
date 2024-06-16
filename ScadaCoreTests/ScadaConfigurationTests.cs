using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ScadaCore.Configuration;
using ScadaCore.Tags.Model;
using System.Collections.Generic;
using System.Linq;

namespace ScadaCoreTests
{
    [TestClass]
    public class ScadaConfigurationTests
    {
        public static ScadaConfiguration ScadaConfiguration { get; private set; }
        public static List<Tag> Tags { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            ScadaConfiguration = new ScadaConfiguration(@"../../scadaTestConfig.xml");
            Tags = ScadaConfiguration.GetTags().ToList();
        }

        [TestMethod]
        public void ShouldReturnCorrectSizeOfList()
        {
            Assert.AreEqual(4, Tags.Count);
        }

        [TestMethod]
        public void TagShouldHaveCorrectAttributes()
        {
            var tag = (AnalogOutputTag)Tags.Where(t => t.Id == "ao1").First();

            Assert.IsNotNull(tag);
            Assert.AreEqual("deabc", tag.IOAddress);
            Assert.AreEqual(15.4, tag.InitialValue);
            Assert.AreEqual(Unit.Kg, tag.Unit);
            Assert.AreEqual(111.11, tag.Value);
        }
    }
}
