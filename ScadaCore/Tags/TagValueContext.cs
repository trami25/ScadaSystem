using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScadaCore.Tags
{
    public class TagValueContext : DbContext
    {
        public DbSet<TagValue> TagValues { get; set; }
    }
}