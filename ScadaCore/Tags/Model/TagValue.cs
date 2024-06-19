using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags.Model
{
    public class TagValue
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TagId { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

        public TagValue()
        {
        }

        public TagValue(int id, string tagId, double value, DateTime timestamp)
        {
            Id = id;
            TagId = tagId;
            Value = value;
            Timestamp = timestamp;
        }
    }
}