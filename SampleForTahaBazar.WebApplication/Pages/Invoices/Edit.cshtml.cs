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
    public class Edit : PageModel
    {
        private readonly ILogger<Edit> _logger;
        private readonly ISaleService _saleService;

        public Edit(ILogger<Edit> logger, ISaleService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        [BindProperty]
        public InvoiceDto Invoice { get; set; }

        public async Task OnGetAsync(int id)
        {
            Invoice = await _saleService.GetInvoiceAsync(id);
        }
        public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
        {
            await _saleService.UpdateInvoiceAsync(Invoice, cancellationToken);
            return RedirectToPage("Details", new { Id = id });
        }
    }
}