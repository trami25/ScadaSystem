using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags
{
    public class TagService
    {
        public readonly TagRepository _repository;
        public readonly TagProcessor _processor;

        public TagService(TagRepository repository, TagProcessor processor)
        {
            _repository = repository;
            _processor = processor;

            InitializeTasks();
        }

        private void InitializeTasks()
        {
            var tags = _repository.GetInputTags().Where(t => t.IsScanOn);

            foreach (var tag in tags)
            {
                _processor.AddTagTask(tag);
            }
        }

        public ICollection<InputTag> GetInputTags()
        {
            return _repository.GetInputTags();
        }

        public ICollection<Tag> GetAll()
        {
            return _repository.GetAll();
        }
    }
}