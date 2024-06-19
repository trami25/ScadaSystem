using ScadaCore.Tags;
using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReportService.svc or ReportService.svc.cs at the Solution Explorer and start debugging.
    public class ReportService : IReportService
    {
        private readonly string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarmsLog1.txt");
        private List<Alarm> alarmsList = new List<Alarm>();

        public ReportService()
        {
            alarmsList = GetAllAlarms();
        }
        

        public void DoWork()
        {
        }

        public List<Alarm> GetAlarmsWithPriority(int priority)
        {
            return alarmsList
                .Where(a => a.Priority == priority)
                .OrderBy(a => a.ActivationTime)
                .ToList();
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();

            foreach (var line in File.ReadLines(fileName))
            {
                //Console.WriteLine(fileName);
                var parts = line.Split(new[] { ", " }, StringSplitOptions.None);
                if (parts.Length == 5)
                {
                    string tagName = parts[0].Split(':')[1].Trim();
                    string type = parts[1].Split(':')[1].Trim();
                    int priority = int.Parse(parts[2].Split(':')[1].Trim());
                    double threshold = double.Parse(parts[3].Split(':')[1].Trim());
                    if (DateTime.TryParseExact(parts[4].Split(new[] { ": " }, StringSplitOptions.None)[1], "dd.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime activationTime))
                    {
                        alarms.Add(new Alarm(tagName, type, priority, threshold) { ActivationTime = activationTime });
                    }
                }
            }

            return alarms;
        }

        public List<Alarm> GetAllAlarmsInTimePeriod(DateTime startTime, DateTime endTime)
        {
            return alarmsList
                .Where(a => a.ActivationTime >= startTime && a.ActivationTime <= endTime)
                .OrderBy(a => a.Priority)
                .ThenBy(a => a.ActivationTime)
                .ToList();
        }

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
                    .Where(t => t.Timestamp >= startTime && t.Timestamp <= endTime)
                    .OrderBy(t => t.Timestamp)
                    .ToList();
                return tagValues;
            }
        }

        public List<TagValue> GetAllValuesForTag(string tagId)
        {
            using (TagValueContext context = new TagValueContext())
            {
                List<TagValue> tagValues = context.TagValues
                    .Where(tv => tv.TagId == tagId)
                    .OrderBy(tv => tv.Value)
                    .ToList();

                return tagValues;
            }
        }

        public List<TagValue> GetLatestAITagsValues()
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
            using (TagValueContext ctx = new TagValueContext())
            {
                TagValue latestDITagValue = ctx.TagValues
                    .Where(tv => tv.TagId.StartsWith("di"))
                    .OrderByDescending(tv => tv.Timestamp)
                    .FirstOrDefault();

                return latestDITagValue;
            }
        }
    }
}
