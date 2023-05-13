using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SampleForTahaBazar.Entities.DataTransferObject;
using SampleForTahaBazar.Services;

namespace SampleForTahaBazar.WebApplication.Pages.Invoices
{
    public class Details : PageModel
    {
        private readonly ILogger<Details> _logger;
        private readonly ISaleService _saleService;

        public Details(ILogger<Details> logger, ISaleService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        public DetailInvoiceModel InvoiceDetails { get; set; }
        public int ItemId { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken = default)
        {
            ItemId = id;
            var invoice = await _saleService.GetInvoiceAsync(id, cancellationToken);
            InvoiceDetails = new()
            {
                Invoice = invoice,
            };
        }

        public class DetailInvoiceModel
        {
            public InvoiceDto Invoice { get; set; }
            public Double? Total => Invoice?.SalesLines.Sum(s => s.NetTotalPrice);
            public Double? GrossTotal => Invoice?.SalesLines.Sum(s => s.GrossTotalPrice);
            public int? ItemCount => Invoice?.SalesLines.Count();
            public Double? Totaltax => Invoice.SalesLines.Sum(s => s.Taxes);
            public Double? TotalDiscount => Invoice.SalesLines.Sum(s => s.Discount);

        }
    }
}