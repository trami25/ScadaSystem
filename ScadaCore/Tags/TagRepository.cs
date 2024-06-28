using ScadaCore.Configuration;
using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags
{
    public class TagRepository
    {
        private readonly List<Tag> _tags;
        private ScadaConfiguration _configuration; 

        public TagRepository(ScadaConfiguration config)
        {
            _configuration = config;
            _tags = config.GetTags().ToList();
        }

        public ICollection<Tag> GetAll()
        {
            return _tags;
        }

        public Tag GetById(string id)
        {
            return _tags.FirstOrDefault(t => t.Id == id);
        }

        public ICollection<InputTag> GetInputTags()
        {
            return _tags.OfType<InputTag>().ToList();
        }

        public void Create(Tag tag)
        {
            var oldTag = _tags.FirstOrDefault(t => t.Id == tag.Id);

            if (oldTag != null)
            {
                throw new ArgumentException("Tag exists");
            }

            _tags.Add(tag);
            _configuration.AddTag(tag);
        }

        public void Delete(string id)
        {
            _tags.RemoveAll(t => t.Id == id);
            _configuration.DeleteTag(id);
        }

        public void Update(string id, Tag tag)
        {
            var oldTag = _tags.FirstOrDefault(t => t.Id == id);
            if (oldTag != null)
            {
                oldTag = tag;
                _configuration.UpdateTag(tag);
            }

        }

        public void AddAlarmToAnalogInputTag(string tagId, Alarm alarm)
        {
            var tag = _tags.OfType<AnalogInputTag>().FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
            {
                throw new ArgumentException("Tag does not exist");
            }

            tag.Alarms.Add(alarm);
            _configuration.AddAlarmToTag(tagId, alarm);
        }

        public void RemoveAlarmFromAnalogInputTag(string tagId, string alarmName)
        {
            var tag = _tags.OfType<AnalogInputTag>().FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
            {
                throw new ArgumentException("Tag does not exist");
            }

            var alarm = tag.Alarms.FirstOrDefault(a => a.TagName == alarmName); // Tag name should maybe be renamed to alarm name or id
            if (alarm == null)
            {
                throw new ArgumentException("Alarm does not exist");
            }

            tag.Alarms.Remove(alarm);
            _configuration.DeleteAlarmFromTag(tagId, alarmName);
        }
    }
}