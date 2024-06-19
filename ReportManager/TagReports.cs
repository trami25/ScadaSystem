using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    public class TagReports
    {
        public TagReports() {}

        public List<TagValue> GetAllTagValues()
        {
            List<TagValue> tagValues = new List<TagValue>();
            using (TagValueContext context = new TagValueContext())
            {
                tagValues = context.TagValues.ToList();
            }

            return tagValues;
            
        }

        public List<TagValue> GetAllTagValuesInTimePeriod(DateTime startTime, DateTime endTime)
        {
            using (TagValueContext context = new TagValueContext())
            {
                List<TagValue> tagValues = context.TagValues
                    .Where(t => t.Timestamp >= startTime &&  t.Timestamp <= endTime)
                    .OrderBy(t => t.Timestamp)
                    .ToList();
                return tagValues;
            }
        }

        public List<TagValue> GetLatestAITagValues()
        {
            using (TagValueContext context = new TagValueContext())
            {
                List<TagValue> aiTagValues = context.TagValues
                    .Where(tv => tv.TagId.StartsWith("ai"))
                    .GroupBy(tv => tv.TagId)
                    .Select(g => g.OrderByDescending(tv => tv.Timestamp).FirstOrDefault())
                    .OrderBy(tv => tv.Timestamp)
                    .ToList();

                return aiTagValues;
            }
        }

        public List<TagValue> GetLatestDITagsValues()
        {
            using (TagValueContext context = new TagValueContext())
            {
                List<TagValue> diTagsValues = context.TagValues
                    .Where(tv => tv.TagId.StartsWith("di"))
                    .GroupBy(tv => tv.TagId)
                    .Select(g => g.OrderByDescending(tv => tv.Timestamp).FirstOrDefault())
                    .OrderBy(tv => tv.Timestamp)
                    .ToList();

                return diTagsValues;
            }
        }

        public TagValue GetLatestValueAmongAllAITags()
        {
            using (TagValueContext ctx = new TagValueContext())
            {
                TagValue latestAITagValue = ctx.TagValues
                    .Where(tv => tv.TagId.StartsWith("ai"))
                    .OrderByDescending(tv => tv.Timestamp)
                    .FirstOrDefault();

                return latestAITagValue;
            }
        }

        public TagValue GetLatestValueAmongAllDITags()
        {
            using ( TagValueContext ctx = new TagValueContext())
            {
                TagValue latestDITagValue = ctx.TagValues
                    .Where(tv => tv.TagId.StartsWith("di"))
                    .OrderByDescending(tv => tv.Timestamp)
                    .FirstOrDefault();

                return latestDITagValue;
            }
        }

        public List<TagValue> GetAllValuesForTag(string tagId)
        {
            using (TagValueContext  context = new TagValueContext())
            {
                List<TagValue> tagValues = context.TagValues
                    .Where(tv => tv.TagId == tagId)
                    .OrderBy(tv => tv.Value)
                    .ToList();

                return tagValues;
            }
        }

        
    }
}
