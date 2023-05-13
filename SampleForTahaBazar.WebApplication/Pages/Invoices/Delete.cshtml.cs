using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SampleForTahaBazar.Services;

namespace SampleForTahaBazar.WebApplication.Pages.Invoices
{
    public class Delete : PageModel
    {
        private readonly ILogger<Delete> _logger;
        private readonly ISaleService _saleService;

        public Delete(ILogger<Delete> logger, ISaleService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        public async Task<IActionResult> OnGetAsync(int id,CancellationToken cancellationToken=default)
        {
            await _saleService.DeleteInvoiceAsync(id,cancellationToken);
            return Redirect("/Invoices");
        }
    }
}