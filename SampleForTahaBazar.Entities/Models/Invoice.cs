using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleForTahaBazar.Entities.Models
{
    public class Invoice : BasicEntity
    {
        public DateTimeOffset InvoiceDate { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SaleLine> SaleLines { get; }
    }
}