using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTDriver
{
    public class RTUnit
    {
        [Key]
        public string Address { get; set; }
        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }
        public double Value { get; set; }
    }
}
