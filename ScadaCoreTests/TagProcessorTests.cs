using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using SimulationDriver;
using System;
using System.Threading;

namespace ScadaCoreTests
{
    [TestClass]
    public class TagProcessorTests
    {
        private TagProcessor _processor;

        [TestInitialize]
        public void Initialize()
        {
            _processor = new TagProcessor();
        }

        [TestMethod]
        public void ShouldAddTask()
        {
            InputTag tag = new DigitalInputTag("test", "test", "R", 4141414141.0, 1000, true, new MainSimulationDriver());
            _processor.AddTagTask(tag);

            Thread.Sleep(tag.ScanTime + 1000);
            Console.WriteLine(tag.Value);
            Assert.IsFalse(tag.Value == 4141414141.0);
        }
    }
}
