using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    public class TrendingService : ITrendingService
    {
        private readonly TagRepository _tagRepository;

        public TrendingService(TagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public ICollection<TagData> GetTags()
        {
            return _tagRepository.GetAll().Select(t => new TagData { Id = t.Id, Value = t.Value }).ToList();
        }
    }
}
