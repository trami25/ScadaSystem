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
        public List<Tag> Tags { get; }

        public TagRepository(ScadaConfiguration config)
        {
            Tags = config.GetTags().ToList();
        }

        public ICollection<Tag> GetAll()
        {
            return Tags;
        }

        public Tag GetById(string id)
        {
            return Tags.FirstOrDefault(t => t.Id == id);
        }

        public void Create(Tag tag)
        {
            var oldTag = Tags.FirstOrDefault(t => t.Id == tag.Id);

            if (oldTag != null)
            {
                throw new ArgumentException("Tag exists");
            }

            Tags.Add(tag);
        }

        public void Delete(string id)
        {
            Tags.RemoveAll(t => t.Id == id);
        }

        public void Update(string id, Tag tag)
        {
            var oldTag = Tags.FirstOrDefault(t => t.Id == id);
            if (oldTag != null)
            {
                oldTag = tag;
            }
        }
    }
}