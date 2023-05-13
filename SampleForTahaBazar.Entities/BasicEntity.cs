using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleForTahaBazar.Entities
{
    public abstract class BasicEntity
    {
        public int Id { get; set; }
        public DateTimeOffset Created {get;set;}=DateTimeOffset.UtcNow;
    }
}