using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SampleForTahaBazar.Entities.DataTransferObject;
using SampleForTahaBazar.Services;

namespace SampleForTahaBazar.WebApplication.Pages.Invoices
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly ISaleService _saleService;
        private readonly IStringLocalizer<Create> _localizer;

        public Create(ILogger<Create> logger, ISaleService saleService, IStringLocalizer<Create> localizer)
        {
            _logger = logger;
            _saleService = saleService;
            _localizer = localizer;
        }


        public void OnGet()
        {

        }
        [BindProperty]
        public InvoiceDto InvoiceDto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { ModelState = ModelState });
            }
            // Console.WriteLine(HttpContext.Request.Form.Count);
            var id = await _saleService.CreateInvoiceAsync(InvoiceDto);
            return RedirectToPage("Index");
        }
    }
}