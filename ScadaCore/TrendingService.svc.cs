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
        private readonly TagService _tagService;

        public TrendingService(TagService tagService)
        {
            _tagService = tagService;
        }

        public ICollection<TagData> GetTags()
        {
            return _tagService.GetInputTags()
                .Select(t => new TagData { Id = t.Id, Value = t.Value })
                .ToList();
        }
    }
}
