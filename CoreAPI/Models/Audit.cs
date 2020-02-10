using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
