using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScadaCore.Configuration;
using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using SimulationDriver;
using System;

namespace ScadaCoreTests
{
    [TestClass]
    public class TagRepositoryTests
    {
        private ScadaConfiguration _configuration;
        private TagRepository _tagRepository;
        private MainSimulationDriver _simulationDriver;

        [TestInitialize]
        public void Setup()
        {
            _simulationDriver = new MainSimulationDriver();
            _configuration = new ScadaConfiguration(@"../../scadaTestConfig.xml", _simulationDriver, _simulationDriver);
            _tagRepository = new TagRepository(_configuration);
        }

        [TestMethod]
        public void ShouldReturnAllTags()
        {
            var tags = _tagRepository.GetAll();

            Assert.AreEqual(4, tags.Count);
        }

        [TestMethod]
        public void ShouldReturnTag()
        {
            var tag = _tagRepository.GetById("ao1");

            Assert.IsNotNull(tag);
        }

        [TestMethod]
        public void ShouldReturnNull()
        {
            var tag = _tagRepository.GetById("aaaa");

            Assert.IsNull(tag);
        }

        [TestMethod]
        public void ShouldCreateTag()
        {
            var tag = new DigitalOutputTag("aaa", "test", "abc", 25.0, 10.0);
            _tagRepository.Create(tag);
            var newTag = _tagRepository.GetById("aaa");

            Assert.IsNotNull(newTag);
            Assert.AreEqual(tag.Id, newTag.Id);
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenCreatingIfTagExists()
        {
            var tag = new DigitalOutputTag("di1", "test", "abc", 25.0, 10.0);

            Assert.ThrowsException<ArgumentException>(() => _tagRepository.Create(tag));
        }

        [TestMethod]
        public void ShouldDeleteTag()
        {
            _tagRepository.Delete("ao1");
            var tags = _tagRepository.GetAll();

            Assert.AreEqual(3, tags.Count);
        }

        [TestMethod]
        public void ShouldUpdateTag()
        {
            var tag = _tagRepository.GetById("do1");
            tag.Value = 50.0;
            _tagRepository.Update(tag.Id, tag);
            var updatedTag = _tagRepository.GetById("do1");

            Assert.IsNotNull(updatedTag);
            Assert.AreEqual(tag.Id, updatedTag.Id);
            Assert.AreEqual(50.0, updatedTag.Value);
        }
    }
}
