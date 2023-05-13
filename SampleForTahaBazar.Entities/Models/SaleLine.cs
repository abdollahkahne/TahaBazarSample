using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleForTahaBazar.Entities.Models
{
    public class SaleLine:BasicEntity
    {
        public virtual Product Product { get; set; }
        public int ProductId {get;set;}
        public virtual UnitOfMeasurement UnitOfMeasurement {get;set;}
        public int UnitOfMeasurementId {get;set;}
        public virtual Invoice Invoice { get; set; }
        public int InvoiceId {get;set;}
        public int Count {get;set;}
        public double UnitPrice {get;set;}
        public double GrossTotalPrice {get;set;}
        public double NetTotalPrice {get;set;}
        public double Discount {get;set;}
        public double Taxes {get;set;}
       
    }
}