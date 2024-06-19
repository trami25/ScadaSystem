using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RTDriver
{
    public class RTUnitContext : DbContext
    {
        public DbSet<RTUnit> Units { get; set; }
    }
}