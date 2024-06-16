using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScadaCore.Configuration;
using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using System;

namespace ScadaCoreTests
{
    [TestClass]
    public class TagRepositoryTests
    {
        private ScadaConfiguration configuration;
        private TagRepository tagRepository;

        [TestInitialize]
        public void Setup()
        {
            configuration = new ScadaConfiguration(@"../../scadaTestConfig.xml");
            tagRepository = new TagRepository(configuration);
        }

        [TestMethod]
        public void ShouldReturnAllTags()
        {
            var tags = tagRepository.GetAll();

            Assert.AreEqual(4, tags.Count);
        }

        [TestMethod]
        public void ShouldReturnTag()
        {
            var tag = tagRepository.GetById("ao1");

            Assert.IsNotNull(tag);
        }

        [TestMethod]
        public void ShouldReturnNull()
        {
            var tag = tagRepository.GetById("aaaa");

            Assert.IsNull(tag);
        }

        [TestMethod]
        public void ShouldCreateTag()
        {
            var tag = new DigitalOutputTag("aaa", "test", "abc", 25.0, 10.0);
            tagRepository.Create(tag);
            var newTag = tagRepository.GetById("aaa");

            Assert.IsNotNull(newTag);
            Assert.AreEqual(tag.Id, newTag.Id);
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenCreatingIfTagExists()
        {
            var tag = new DigitalOutputTag("di1", "test", "abc", 25.0, 10.0);

            Assert.ThrowsException<ArgumentException>(() => tagRepository.Create(tag));
        }

        [TestMethod]
        public void ShouldDeleteTag()
        {
            tagRepository.Delete("ao1");
            var tags = tagRepository.GetAll();

            Assert.AreEqual(3, tags.Count);
        }

        [TestMethod]
        public void ShouldUpdateTag()
        {
            var tag = tagRepository.GetById("do1");
            tag.Value = 50.0;
            tagRepository.Update(tag.Id, tag);
            var updatedTag = tagRepository.GetById("do1");

            Assert.IsNotNull(updatedTag);
            Assert.AreEqual(tag.Id, updatedTag.Id);
            Assert.AreEqual(50.0, updatedTag.Value);
        }
    }
}
