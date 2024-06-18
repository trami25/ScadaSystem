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

        public Tag GetById(string id)
        {
            return _repository.GetById(id);
        }

        public void Create(Tag tag)
        {
            _repository.Create(tag);

            var inputTag = tag as InputTag;
            if (inputTag != null && inputTag.IsScanOn)
            {
                _processor.AddTagTask(inputTag);
            }
        }

        public void Update(string id, Tag tag)
        {
            _repository.Update(id, tag);

            var inputTag = tag as InputTag;
            if (inputTag != null)
            {
                if (inputTag.IsScanOn)
                {
                    _processor.AddTagTask(inputTag);
                }
                else
                {
                    _processor.DeleteTagTask(inputTag.Id);
                }
            }
        }

        public void Delete(string id)
        {
            _repository.Delete(id);
            _processor.DeleteTagTask(id);
        }

        public void AddAlarmToAnalogInputTag(string id, Alarm alarm)
        {
            _repository.AddAlarmToAnalogInputTag(id, alarm);
        }

        public void RemoveAlarmFromAnalogInputTag(string tagId, string alarmName)
        {
            _repository.RemoveAlarmFromAnalogInputTag(tagId, alarmName);
        }
    }
}